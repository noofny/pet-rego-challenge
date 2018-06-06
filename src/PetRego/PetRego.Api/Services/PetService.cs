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
    public class PetService
    {
        readonly IRepository<PetEntity> PetRepository;
        readonly IAppConfig AppConfig;

        public PetService(IRepository<PetEntity> petRepository, IAppConfig appConfig)
        {
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
                Link.Custom("detail", $"{AppConfig.TokenizedBaseUrl}/{{id}}/detail", HttpMethod.Get.Method),
            });

            try
            {
                var petEntities = await PetRepository.Search(field, value);
                var petModels = Mapper.Map<List<PetModel>>(petEntities);
                return new MultiResponse(Result.Success, metadata, petModels.OfType<IModel>().ToList());
            }
            catch (DataException<PetEntity> ex)
            {
                return new MultiResponse(ex.Result, metadata, $"{ex.Message}");
            }
            catch (Exception ex)
            {
                return new MultiResponse(Result.InternalError, metadata, $"{ex.GetBaseException().Message}");
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
                // todo - provide an action template instructing how to create a new item
                // todo - provide an action template instructing how to update an existing item
            });

            try
            {
                var petEntity = await PetRepository.Get(id);
                var petModel = Mapper.Map<PetModel>(petEntity);
                return new SingleResponse(Result.Success, metadata, petModel);
            }
            catch (DataException<PetEntity> ex)
            {
                return new SingleResponse(ex.Result, metadata, $"{ex.Message}");
            }
            catch (Exception ex)
            {
                return new SingleResponse(Result.InternalError, metadata, $"{ex.GetBaseException().Message}");
            }
        }

        public async Task<IResponse> Create(PetModel pet)
        {
            if (pet == null)
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Post.Method)), $"Parameter cannot be null : owner");
            }
            if (string.IsNullOrEmpty(pet.Id))
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Post.Method)), $"Parameter cannot be null : owner.id");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Post.Method),
                Link.Custom("detail", $"{AppConfig.TokenizedBaseUrl}/{pet.Id}/detail", HttpMethod.Get.Method),
                Link.Edit($"{AppConfig.TokenizedBaseUrl}/{pet.Id}", HttpMethod.Put.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}/{pet.Id}", HttpMethod.Delete.Method),
            });

            try
            {
                var petEntity = Mapper.Map<PetEntity>(pet);
                var added = await PetRepository.Add(petEntity);
                return new Response(added ? Result.Created : Result.Noop, metadata);
            }
            catch (DataException<PetEntity> ex)
            {
                return new Response(ex.Result, metadata, $"{ex.Message}");
            }
            catch (Exception ex)
            {
                return new Response(Result.InternalError, metadata, $"{ex.GetBaseException().Message}");
            }
        }

        public async Task<IResponse> Update(PetModel pet)
        {
            if (pet == null)
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Put.Method)), $"Parameter cannot be null : owner");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Put.Method),
                Link.Custom("detail", $"{AppConfig.TokenizedBaseUrl}/{pet.Id}/detail", HttpMethod.Get.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}/{pet.Id}", HttpMethod.Delete.Method),
            });

            try
            {
                var petEntity = Mapper.Map<PetEntity>(pet);
                var updated = await PetRepository.Update(petEntity);
                return new Response(updated ? Result.Updated : Result.Noop, metadata);
            }
            catch (DataException<PetEntity> ex)
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
                var deleted = await PetRepository.Delete(id);
                return new Response(deleted ? Result.Deleted : Result.Noop, metadata);
            }
            catch (DataException<PetEntity> ex)
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
