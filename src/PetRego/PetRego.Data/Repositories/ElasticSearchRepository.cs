using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Nest;
using Nest.JsonNetSerializer;
using Elasticsearch.Net;
using PetRego.Common;
using AutoMapper;

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
        readonly string _defaultTypeName;
        const long WriteConflictRetries = 2;

        public ElasticSearchRepository(ElasticClientFactory<T> clientFactory, IAppConfig appConfig)
        {
            _client = clientFactory.GetClient(appConfig.ElasticSearchUri);
            _defaultIndex = _client.ConnectionSettings != null ? _client.ConnectionSettings.DefaultIndex : string.Empty;
            _defaultTypeName = _client.ConnectionSettings != null ? _client.ConnectionSettings.DefaultTypeName : string.Empty;
        }
        public ElasticSearchRepository(IElasticClient client)
        {
            _client = client;
            _defaultIndex = _client.ConnectionSettings != null ? _client.ConnectionSettings.DefaultIndex : string.Empty;
            _defaultTypeName = _client.ConnectionSettings != null ? _client.ConnectionSettings.DefaultTypeName : string.Empty;
        }




        public async Task<T> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var existsResponse = await _client.DocumentExistsAsync<T>(id, d => d.Index(_defaultIndex).Type(_defaultTypeName));
            if (!existsResponse.Exists)
            {
                return default(T);
            }
            var response = await _client.GetAsync<T>(id, s => s.Index(_defaultIndex).Type(_defaultTypeName));
            if (!response.IsValid)
            {
                throw new DataException<T>(
                    "Get",
                    Mapper.Map<Models.Result>(Result.Error),
                    $"{response.OriginalException.Message}{Environment.NewLine}{response.ServerError}{Environment.NewLine}{response.DebugInformation}",
                    response.OriginalException
                );
            }
            return response.Source;
        }

        // todo - add pagination
        public async Task<List<T>> Search(string field, string value)
        {
            var indexResponse = await _client.IndexExistsAsync(_defaultIndex);
            if (!indexResponse.IsValid)
            {
                throw new DataException<T>(
                    "List(index check)",
                    Mapper.Map<Models.Result>(Result.Error),
                    $"{indexResponse.OriginalException.Message}{Environment.NewLine}{indexResponse.ServerError}{Environment.NewLine}{indexResponse.DebugInformation}",
                    indexResponse.OriginalException
                );
            }
            if (!indexResponse.Exists)
            {
                // Index doesn't exist - return an empty response.
                return new List<T>();
            }
            var searchResponse = _client.Search<T>(s => s
                .Index(_client.ConnectionSettings.DefaultIndex)
                .Type(_client.ConnectionSettings.DefaultTypeName)
                .Query(q => q.QueryString(d => d.Query($"{field}={value}"))));
            if (!searchResponse.IsValid)
            {
                throw new DataException<T>(
                    "Search",
                    Mapper.Map<Models.Result>(Result.Error),
                    $"{searchResponse.OriginalException.Message}{Environment.NewLine}{searchResponse.ServerError}{Environment.NewLine}{searchResponse.DebugInformation}",
                    searchResponse.OriginalException
                );
            }
            return searchResponse.Documents.ToList();
        }

        public async Task<bool> Add(T entity)
        {
            entity.Created = DateTime.UtcNow;
            var response = await _client.IndexDocumentAsync(entity);
            if (response.Result != Result.Created || !response.IsValid)
            {
                throw new DataException<T>(
                    "Add",
                    Mapper.Map<Models.Result>(response.Result),
                    $"{response.OriginalException.Message}{Environment.NewLine}{response.ServerError}{Environment.NewLine}{response.DebugInformation}",
                    response.OriginalException
                );
            }
            return true;
        }

        public async Task<bool> Update(T entity)
        {
            var existsResponse = await _client.DocumentExistsAsync<T>(entity.Id, d => d.Index(_defaultIndex).Type(_defaultTypeName));
            if (!existsResponse.Exists)
            {
                return false;
            }
            entity.Updated = DateTime.UtcNow;
            var response = await _client.UpdateAsync<T, object>(entity.Id, u => u
                .Doc(entity)
                .Upsert(entity)
                .RetryOnConflict(WriteConflictRetries) // brute force optimistic concurrency
            );
            if (response.Result != Result.Updated || !response.IsValid)
            {
                throw new DataException<T>(
                    "Update",
                    Mapper.Map<Models.Result>(response.Result),
                    $"{response.OriginalException.Message}{Environment.NewLine}{response.ServerError}{Environment.NewLine}{response.DebugInformation}",
                    response.OriginalException
                );
            }
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            var existsResponse = await _client.DocumentExistsAsync<T>(id, d => d.Index(_defaultIndex).Type(_defaultTypeName));
            if (!existsResponse.Exists)
            {
                return false;
            }
            var indexResponse = await _client.IndexExistsAsync(_defaultIndex);
            if (!indexResponse.IsValid)
            {
                throw new DataException<T>(
                    "Delete(inded check)",
                    Mapper.Map<Models.Result>(Result.Error),
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
                    Mapper.Map<Models.Result>(deleteResponse.Result),
                    $"{deleteResponse.OriginalException.Message}{Environment.NewLine}{deleteResponse.ServerError}{Environment.NewLine}{deleteResponse.DebugInformation}",
                    deleteResponse.OriginalException
                );
            }
            return true;
        }

    }
}
