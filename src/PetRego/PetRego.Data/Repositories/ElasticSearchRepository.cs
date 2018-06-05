using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Nest;
using Nest.JsonNetSerializer;
using Elasticsearch.Net;
using PetRego.Common;

namespace PetRego.Data
{
    public class ElasticClientFactory<T> where T : IEntity
    {
        const string IndexPrefix = "petrego";

        public IElasticClient GetClient(string instanceUri)
        {
            if (string.IsNullOrEmpty(instanceUri))
            {
                throw new ArgumentNullException(nameof(instanceUri), $"This parameter must be provided!");
            }

            var defaultIndex = $"{IndexPrefix}-{typeof(T).Name}".ToLower();
            var defaultTypeName = $"{typeof(T).Name}";

            var connectionPool = new SingleNodeConnectionPool(new Uri(instanceUri));
            var connectionSettings = new ConnectionSettings(connectionPool, JsonNetSerializer.Default)
                .DefaultIndex(defaultIndex)
                .DefaultTypeName(defaultTypeName);
            connectionSettings.DisableDirectStreaming();
            var client = new ElasticClient(connectionSettings);
            return client;
        }
    }


    public class ElasticSearchRepository<T> : IRepository<T> where T : class, IEntity
    {
        readonly IElasticClient _client;
        readonly string _defaultIndex;
        const long WriteConflictRetries = 2;

        public ElasticSearchRepository(ElasticClientFactory<T> clientFactory, IAppConfig appConfig)
        {
            _client = clientFactory.GetClient(appConfig.ElasticSearchUri);
            _defaultIndex = _client.ConnectionSettings != null ? _client.ConnectionSettings.DefaultIndex : string.Empty;
        }
        public ElasticSearchRepository(IElasticClient client)
        {
            _client = client;
            _defaultIndex = _client.ConnectionSettings != null ? _client.ConnectionSettings.DefaultIndex : string.Empty;
        }




        public async Task<T> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var response = await _client.SearchAsync<T>(s => s
                .Query(q => q.Match(m => m.Field(f => f.Id).Query(id)))
            );
            if (!response.IsValid)
            {
                throw new DataException<T>(
                    "Get",
                    MapNestResult(Result.Error),
                    $"{response.OriginalException.Message}{Environment.NewLine}{response.ServerError}{Environment.NewLine}{response.DebugInformation}",
                    response.OriginalException
                );
            }
            return response.Documents.SingleOrDefault();
        }

        public async Task<List<T>> List()
        {
            var indexResponse = await _client.IndexExistsAsync(_defaultIndex);
            if (!indexResponse.IsValid)
            {
                throw new DataException<T>(
                    "List(index check)",
                    MapNestResult(Result.Error),
                    $"{indexResponse.OriginalException.Message}{Environment.NewLine}{indexResponse.ServerError}{Environment.NewLine}{indexResponse.DebugInformation}",
                    indexResponse.OriginalException
                );
            }
            if (!indexResponse.Exists)
            {
                // Index doesn't exist - return an empty response.
                return new List<T>();
            }
            var listResponse = _client.Search<T>(s => s
                 .Index(_client.ConnectionSettings.DefaultIndex)
                 .Type(_client.ConnectionSettings.DefaultTypeName));
            if (!listResponse.IsValid)
            {
                throw new DataException<T>(
                    "List",
                    MapNestResult(Result.Error),
                    $"{listResponse.OriginalException.Message}{Environment.NewLine}{listResponse.ServerError}{Environment.NewLine}{listResponse.DebugInformation}",
                    listResponse.OriginalException
                );
            }
            return listResponse.Documents.ToList();
        }

        public async Task<bool> Add(T entity)
        {
            var response = await _client.IndexDocumentAsync(new OwnerEntity());
            if (response.Result != Result.Created || !response.IsValid)
            {
                throw new DataException<T>(
                    "Add",
                    MapNestResult(response.Result),
                    $"{response.OriginalException.Message}{Environment.NewLine}{response.ServerError}{Environment.NewLine}{response.DebugInformation}",
                    response.OriginalException
                );
            }
            return true;
        }

        public async Task<bool> Update(T entity)
        {
            var response = await _client.UpdateAsync<T, object>(entity.Id, u => u
                .Doc(entity)
                .Upsert(entity)
                .RetryOnConflict(WriteConflictRetries) // brute force optimistic concurrency
            );
            if (response.Result != Result.Updated || !response.IsValid)
            {
                throw new DataException<T>(
                    "Update",
                    MapNestResult(response.Result),
                    $"{response.OriginalException.Message}{Environment.NewLine}{response.ServerError}{Environment.NewLine}{response.DebugInformation}",
                    response.OriginalException
                );
            }
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            var indexResponse = await _client.IndexExistsAsync(_defaultIndex);
            if (!indexResponse.IsValid)
            {
                throw new DataException<T>(
                    "Delete(inded check)",
                    MapNestResult(Result.Error),
                    $"{indexResponse.OriginalException.Message}{Environment.NewLine}{indexResponse.ServerError}{Environment.NewLine}{indexResponse.DebugInformation}",
                    indexResponse.OriginalException
                );
            }
            if (!indexResponse.Exists)
            {
                // index doesn't exist - nothing to do
                return false;
            }
            var deleteResponse = await _client.DeleteAsync<T>(id);
            if (!deleteResponse.IsValid && deleteResponse.Result != Result.NotFound)
            {
                throw new DataException<T>(
                    "Delete",
                    MapNestResult(deleteResponse.Result),
                    $"{deleteResponse.OriginalException.Message}{Environment.NewLine}{deleteResponse.ServerError}{Environment.NewLine}{deleteResponse.DebugInformation}",
                    deleteResponse.OriginalException
                );
            }
            return true;
        }

        public async Task<bool> DeleteAll()
        {
            var indexResponse = await _client.IndexExistsAsync(_defaultIndex);
            if (!indexResponse.IsValid)
            {
                throw new DataException<T>(
                    "DeleteAll(index check)",
                    MapNestResult(Result.Error),
                    $"{indexResponse.OriginalException.Message}{Environment.NewLine}{indexResponse.ServerError}{Environment.NewLine}{indexResponse.DebugInformation}",
                    indexResponse.OriginalException
                );
            }
            if (!indexResponse.Exists)
            {
                // index doesn't exist - nothing to do
                return false;
            }
            var deleteResponse = await _client.DeleteIndexAsync(_defaultIndex);
            if (!deleteResponse.IsValid)
            {
                throw new DataException<T>(
                    "DeleteAll",
                    MapNestResult(Result.Error),
                    $"{deleteResponse.OriginalException.Message}{Environment.NewLine}{deleteResponse.ServerError}{Environment.NewLine}{deleteResponse.DebugInformation}",
                    deleteResponse.OriginalException
                );
            }
            return true;
        }



       
        /// <summary>
        /// Todo - might be better to leverage Automapper here?
        /// This is okay for now as it provides an explicit result and is transparent
        /// in the case if NEST adds new values which I haven't supported.
        /// </summary>
        /// <returns>The value from the NEST library.</returns>
        /// <param name="result">The relative mapped value to this API.</param>
        Models.Result MapNestResult(Result result)
        {
            switch (result)
            {
                case Result.Created:
                    return Models.Result.Created;

                case Result.Deleted:
                    return Models.Result.Deleted;

                case Result.Error:
                    return Models.Result.InternalError;

                case Result.Noop:
                    return Models.Result.Noop;

                case Result.NotFound:
                    return Models.Result.NotFound;

                case Result.Updated:
                    return Models.Result.Updated;

                default:
                    return Models.Result.Unsupported;
            }
        }


    }
}
