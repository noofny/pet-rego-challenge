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

        public async Task<IResponse> Search(string emailAddress)
        {
            var metadata = new Metadata(new List<Link>
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Get.Method),
                Link.Custom("summary", $"{AppConfig.TokenizedBaseUrl}/{{id}}", HttpMethod.Get.Method),
                Link.Custom("detail", $"{AppConfig.TokenizedBaseUrl}/{{id}}", HttpMethod.Get.Method),
            }, AppConfig.TokenizedBaseUrl);

            try
            {
                var ownerEntities = await OwnerRepository.Search("emailAddress", emailAddress);
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
            var metadata = new Metadata(new List<Link>
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}/{id}", HttpMethod.Get.Method),
                Link.Edit($"{AppConfig.TokenizedBaseUrl}/{id}", HttpMethod.Put.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}/{id}", HttpMethod.Delete.Method),
                Link.Custom("detail", $"{AppConfig.TokenizedBaseUrl}/{id}", HttpMethod.Get.Method),
                Link.Custom("summary", $"{AppConfig.TokenizedBaseUrl}/{id}", HttpMethod.Get.Method),
                Link.Custom("pets", $"{AppConfig.TokenizedBaseUrl}/{id}/pets", HttpMethod.Get.Method),
                // todo - provide an action template instructing how to create a new item
                // todo - provide an action template instructing how to update an existing item
            }, AppConfig.TokenizedBaseUrl);

            if (string.IsNullOrEmpty(id))
            {
                return new SingleResponse(Result.BadRequest, metadata, $"Parameter cannot be null : id");
            }

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
            var metadata = new Metadata(new List<Link>
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}/{id}", HttpMethod.Get.Method),
                Link.Edit($"{AppConfig.TokenizedBaseUrl}/{id}", HttpMethod.Put.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}/{id}", HttpMethod.Delete.Method),
                Link.Custom("summary", $"{AppConfig.TokenizedBaseUrl}/{id}", HttpMethod.Get.Method),
                Link.Custom("pets", $"{AppConfig.TokenizedBaseUrl}/{id}/pets", HttpMethod.Get.Method),
                // todo - provide an action template instructing how to create a new item
                // todo - provide an action template instructing how to update an existing item
            }, AppConfig.TokenizedBaseUrl);

            if (string.IsNullOrEmpty(id))
            {
                return new SingleResponse(Result.BadRequest, metadata, $"Parameter cannot be null : id");
            }

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
            var metadata = new Metadata(new List<Link>
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Post.Method),
                Link.Custom("summary", $"{AppConfig.TokenizedBaseUrl}/{owner.Id}", HttpMethod.Get.Method),
                Link.Custom("detail", $"{AppConfig.TokenizedBaseUrl}/{owner.Id}", HttpMethod.Get.Method),
                Link.Edit($"{AppConfig.TokenizedBaseUrl}/{owner.Id}", HttpMethod.Put.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}/{owner.Id}", HttpMethod.Delete.Method),
            }, AppConfig.TokenizedBaseUrl);

            if (owner == null)
            {
                return new SingleResponse(Result.BadRequest, metadata, $"Parameter cannot be null : owner");
            }
            if (string.IsNullOrEmpty(owner.Id))
            {
                return new SingleResponse(Result.BadRequest, metadata, $"Parameter cannot be null : owner.id");
            }

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
            var metadata = new Metadata(new List<Link>
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Put.Method),
                Link.Custom("summary", $"{AppConfig.TokenizedBaseUrl}/{owner.Id}", HttpMethod.Get.Method),
                Link.Custom("detail", $"{AppConfig.TokenizedBaseUrl}/{owner.Id}", HttpMethod.Get.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}/{owner.Id}", HttpMethod.Delete.Method),
            }, AppConfig.TokenizedBaseUrl);

            if (owner == null)
            {
                return new SingleResponse(Result.BadRequest, metadata, $"Parameter cannot be null : owner");
            }

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
            var metadata = new Metadata(new List<Link>
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}/{id}", HttpMethod.Delete.Method),
                Link.Custom("search", $"{AppConfig.TokenizedBaseUrl}/search/{{emailAddress}}", HttpMethod.Get.Method),
            }, AppConfig.TokenizedBaseUrl);

            if (string.IsNullOrEmpty(id))
            {
                return new SingleResponse(Result.BadRequest, metadata, $"Parameter cannot be null : id");
            }

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
