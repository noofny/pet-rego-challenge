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
    public class OwnerService 
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

        public async Task<IResponse> Search(string field, string value)
        {
            // todo - these min param length contraints are present because of the 
            //        way I hacked together the search method in the ElasticSearch repo.
            //        I could improve that so that the contraints are not needed.
            const int MinParamLength = 4;
            if (string.IsNullOrEmpty(field))
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Get.Method)), $"Parameter cannot be null : field");
            }
            if (field.Length < MinParamLength)
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Get.Method)), $"Parameter 'field' length must be {MinParamLength} or greater");
            }
            if (string.IsNullOrEmpty(value))
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Get.Method)), $"Parameter cannot be null : value");
            }
            if (value.Length < MinParamLength)
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Get.Method)), $"Parameter 'value' length must be {MinParamLength} or greater");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Get.Method),
                Link.Custom("summary", $"{AppConfig.TokenizedBaseUrl}/{{id}}/summary", HttpMethod.Get.Method),
                Link.Custom("detail", $"{AppConfig.TokenizedBaseUrl}/{{id}}/detail", HttpMethod.Get.Method),
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

        public async Task<IResponse> Summary(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Get.Method)), $"Parameter cannot be null : id");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Get.Method),
                Link.Edit($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Put.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Delete.Method),
                Link.Custom("detail", $"{AppConfig.TokenizedBaseUrl}/detail", HttpMethod.Get.Method),
                Link.Custom("pets", $"{AppConfig.TokenizedBaseUrl}/pets", HttpMethod.Get.Method),
                // todo - provide an action template instructing how to create a new item
                // todo - provide an action template instructing how to update an existing item
            });

            try
            {
                var ownerEntity = await OwnerRepository.Get(id);
                var petEntities = await PetRepository.List(); // todo - Search by OwnerId
                var ownerSummaryModel = Mapper.Map<OwnerSummaryModel>(ownerEntity);
                ownerSummaryModel.NumberOfPets = petEntities.Count; // todo - move to mapper is possibl
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

        public async Task<IResponse> Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Get.Method)), $"Parameter cannot be null : id");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Get.Method),
                Link.Edit($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Put.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Delete.Method),
                Link.Custom("summary", $"{AppConfig.TokenizedBaseUrl}/summary", HttpMethod.Get.Method),
                Link.Custom("pets", $"{AppConfig.TokenizedBaseUrl}/pets", HttpMethod.Get.Method),
                // todo - provide an action template instructing how to create a new item
                // todo - provide an action template instructing how to update an existing item
            });

            try
            {
                var ownerEntity = await OwnerRepository.Get(id);
                var petEntities = await PetRepository.List(); // todo - Search by OwnerId
                var ownerDetailModel = Mapper.Map<OwnerDetailModel>(ownerEntity);
                ownerDetailModel.NumberOfPets = petEntities.Count; // todo - move to mapper is possibl
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

        public async Task<IResponse> Create(OwnerDetailModel owner)
        {
            if (owner == null)
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Post.Method)), $"Parameter cannot be null : owner");
            }
            if (string.IsNullOrEmpty(owner.Id))
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Post.Method)), $"Parameter cannot be null : owner.id");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Post.Method),
                Link.Custom("summary", $"{AppConfig.TokenizedBaseUrl}/{owner.Id}/summary", HttpMethod.Get.Method),
                Link.Custom("detail", $"{AppConfig.TokenizedBaseUrl}/{owner.Id}/detail", HttpMethod.Get.Method),
                Link.Edit($"{AppConfig.TokenizedBaseUrl}/{owner.Id}", HttpMethod.Put.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}/{owner.Id}", HttpMethod.Delete.Method),
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

        public async Task<IResponse> Update(OwnerDetailModel owner)
        {
            if (owner == null)
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Put.Method)), $"Parameter cannot be null : owner");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Put.Method),
                Link.Custom("summary", $"{AppConfig.TokenizedBaseUrl}/{owner.Id}/summary", HttpMethod.Get.Method),
                Link.Custom("detail", $"{AppConfig.TokenizedBaseUrl}/{owner.Id}/detail", HttpMethod.Get.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}/{owner.Id}", HttpMethod.Delete.Method),
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

        public async Task<IResponse> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Delete.Method)), $"Parameter cannot be null : id");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Delete.Method),
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
