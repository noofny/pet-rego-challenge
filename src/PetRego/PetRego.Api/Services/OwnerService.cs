using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using PetRego.Data;
using PetRego.Models;
using PetRego.Common;
using AutoMapper;

namespace PetRego.Api
{
    public class OwnerService : Service
    {
        readonly IRepository<OwnerEntity> OwnerRepository;
        readonly IRepository<PetEntity> PetRepository;
        readonly IAppConfig AppConfig;

        public OwnerService(IRepository<OwnerEntity> ownerRepository, IRepository<PetEntity> petRepository, IAppConfig appConfig)
        {
            OwnerRepository = ownerRepository;
            PetRepository = petRepository;
            AppConfig = appConfig;
        }

        public async Task<MultiResponse> Search(string field, string value)
        {
            // todo - these min param length contraints are present because of the 
            //        way I hacked together the search method in the ElasticSearch repo.
            //        I could improve that so that the contraints are not needed.
            const int MinParamLength = 4;
            if (string.IsNullOrEmpty(field))
            {
                return new MultiResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Get.Method)), $"Parameter cannot be null : field");
            }
            if (field.Length < MinParamLength)
            {
                return new MultiResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Get.Method)), $"Parameter 'field' length must be {MinParamLength} or greater");
            }
            if (string.IsNullOrEmpty(value))
            {
                return new MultiResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Get.Method)), $"Parameter cannot be null : value");
            }
            if (value.Length < MinParamLength)
            {
                return new MultiResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Get.Method)), $"Parameter 'value' length must be {MinParamLength} or greater");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Get.Method),
                Link.Custom("summary", $"{AppConfig.TokenizedCurrentUrl}/{{id}}/summary", HttpMethod.Get.Method),
                Link.Custom("detail", $"{AppConfig.TokenizedCurrentUrl}/{{id}}/detail", HttpMethod.Get.Method),
            });

            try
            {
                var ownerEntities = await OwnerRepository.Search(field, value);
                var ownerSummaryModels = Mapper.Map<List<OwnerSummaryModel>>(ownerEntities);
                return new MultiResponse(Result.Success, metadata, ownerSummaryModels.OfType<IModel>().ToList());
            }
            catch(DataException<OwnerEntity> ex)
            {
                return new MultiResponse(ex.Result, metadata, $"{ex.Message}");
            }
            catch (Exception ex)
            {
                return new MultiResponse(Result.InternalError, metadata, $"{ex.GetBaseException().Message}");
            }
        }

        public async Task<SingleResponse> Summary(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Get.Method)), $"Parameter cannot be null : id");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Get.Method),
                Link.Edit($"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}{AppConfig.TokenizedControllerPath}/{id}", HttpMethod.Put.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}{AppConfig.TokenizedControllerPath}/{id}", HttpMethod.Delete.Method),
                Link.Custom("detail", $"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}{AppConfig.TokenizedControllerPath}/{id}/detail", HttpMethod.Get.Method),
                Link.Custom("pets", $"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}pet/search/ownerId/{id}", HttpMethod.Get.Method),
                // todo - provide an action template instructing how to create a new item
                // todo - provide an action template instructing how to update an existing item
            });

            try
            {
                var ownerEntity = await OwnerRepository.Get(id);
                var petEntities = await PetRepository.Search("ownerId", id); // todo - get by foreign key/index lookup
                var ownerSummaryModel = Mapper.Map<OwnerSummaryModel>(ownerEntity);
                return new SingleResponse(Result.Success, metadata, ownerSummaryModel);
            }
            catch (DataException<OwnerEntity> ex)
            {
                return new SingleResponse(ex.Result, metadata, $"{ex.Message}");
            }
            catch (Exception ex)
            {
                return new SingleResponse(Result.InternalError, metadata, $"{ex.GetBaseException().Message}");
            }
        }

        public async Task<SingleResponse> Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Get.Method)), $"Parameter cannot be null : id");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Get.Method),
                Link.Edit($"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}{AppConfig.TokenizedControllerPath}/{id}", HttpMethod.Put.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}{AppConfig.TokenizedControllerPath}/{id}", HttpMethod.Delete.Method),
                Link.Custom("summary", $"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}{AppConfig.TokenizedControllerPath}/{id}/summary", HttpMethod.Get.Method),
                Link.Custom("pets", $"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}pet/search/ownerId/{id}", HttpMethod.Get.Method),
                // todo - provide an action template instructing how to create a new item
                // todo - provide an action template instructing how to update an existing item
            });

            try
            {
                var ownerEntity = await OwnerRepository.Get(id);
                var petEntities = await PetRepository.Search("ownerId", id); // todo - get by foreign key/index lookup
                foreach(var petEntity in petEntities)
                {
                    metadata.Links.Add(Link.Custom("pet", $"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}pet/{petEntity.Id}/detail", HttpMethod.Get.Method));
                }
                var ownerDetailModel = Mapper.Map<OwnerDetailModel>(ownerEntity);
                return new SingleResponse(Result.Success, metadata, ownerDetailModel);
            }
            catch (DataException<OwnerEntity> ex)
            {
                return new SingleResponse(ex.Result, metadata, $"{ex.Message}");
            }
            catch (Exception ex)
            {
                return new SingleResponse(Result.InternalError, metadata, $"{ex.GetBaseException().Message}");
            }
        }

        public async Task<Response> Create(OwnerDetailModel owner)
        {
            if (owner == null)
            {
                return new Response(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Post.Method)), $"Parameter cannot be null : owner");
            }
            if (string.IsNullOrEmpty(owner.Id))
            {
                return new Response(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Post.Method)), $"Parameter cannot be null : owner.id");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Post.Method),
                Link.Custom("summary", $"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}{AppConfig.TokenizedControllerPath}/{owner.Id}/summary", HttpMethod.Get.Method),
                Link.Custom("detail", $"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}{AppConfig.TokenizedControllerPath}/{owner.Id}/detail", HttpMethod.Get.Method),
                Link.Edit($"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}{AppConfig.TokenizedControllerPath}/{owner.Id}", HttpMethod.Put.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}{AppConfig.TokenizedControllerPath}/{owner.Id}", HttpMethod.Delete.Method),
            });

            try
            {
                var entity = Mapper.Map<OwnerEntity>(owner);
                var added = await OwnerRepository.Add(entity);
                return new Response(added ? Result.Created : Result.Noop, metadata);
            }
            catch (DataException<OwnerEntity> ex)
            {
                return new Response(ex.Result, metadata, $"{ex.Message}");
            }
            catch (Exception ex)
            {
                return new Response(Result.InternalError, metadata, $"{ex.GetBaseException().Message}");
            }
        }

        public async Task<Response> Update(OwnerDetailModel owner)
        {
            if (owner == null)
            {
                return new Response(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Put.Method)), $"Parameter cannot be null : owner");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Put.Method),
                Link.Custom("summary", $"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}{AppConfig.TokenizedControllerPath}/{owner.Id}/summary", HttpMethod.Get.Method),
                Link.Custom("detail", $"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}{AppConfig.TokenizedControllerPath}/{owner.Id}/detail", HttpMethod.Get.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}/{API_ROUTE_BASE_PATH}{AppConfig.TokenizedControllerPath}/{owner.Id}", HttpMethod.Delete.Method),
            });

            try
            {
                var entity = Mapper.Map<OwnerEntity>(owner);
                var updated = await OwnerRepository.Update(entity);
                return new Response(updated ? Result.Updated : Result.Noop, metadata);
            }
            catch (DataException<OwnerEntity> ex)
            {
                return new Response(ex.Result, metadata, $"{ex.Message}");
            }
            catch (Exception ex)
            {
                return new Response(Result.InternalError, metadata, $"{ex.GetBaseException().Message}");
            }
        }

        public async Task<Response> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new Response(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Delete.Method)), $"Parameter cannot be null : id");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Delete.Method),
            });

            try
            {
                var deleted = await OwnerRepository.Delete(id);
                return new Response(deleted ? Result.Deleted : Result.Noop, metadata);
            }
            catch (DataException<OwnerEntity> ex)
            {
                return new Response(ex.Result, metadata, $"{ex.Message}");
            }
            catch (Exception ex)
            {
                return new Response(Result.InternalError, metadata, $"{ex.GetBaseException().Message}");
            }
        }


    }
}
