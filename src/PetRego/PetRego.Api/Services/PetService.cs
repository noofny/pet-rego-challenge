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

        public async Task<MultiResponse> Search(string field, string value)
        {
            // todo - these min param length contraints are present because of the 
            //        way I hacked together the search method in the ElasticSearch repo.
            //        I could improve that so that the contraints are not needed.
            const int MinParamLength = 4;
            if (string.IsNullOrEmpty(field))
            {
                return new MultiResponse(Result.BadRequest, new Metadata(Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method)), $"Parameter cannot be null : field");
            }
            if (field.Length < MinParamLength)
            {
                return new MultiResponse(Result.BadRequest, new Metadata(Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method)), $"Parameter 'field' length must be {MinParamLength} or greater");
            }
            if (string.IsNullOrEmpty(value))
            {
                return new MultiResponse(Result.BadRequest, new Metadata(Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method)), $"Parameter cannot be null : value");
            }
            if (value.Length < MinParamLength)
            {
                return new MultiResponse(Result.BadRequest, new Metadata(Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method)), $"Parameter 'value' length must be {MinParamLength} or greater");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method),
            });

            try
            {
                var petEntities = await PetRepository.Search(field, value);
                var petModels = Mapper.Map<List<PetModel>>(petEntities);
                foreach(var petEntity in petEntities)
                {
                    metadata.Links.Add(Link.Custom("detail", $"{Constants.TOKENIZED_BASE_URL}/{Constants.API_ROUTE_BASE_PATH}{Constants.TOKENIZED_CONTROLLER_PATH}/{petEntity.Id}/detail", HttpMethod.Get.Method));
                }
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

        public async Task<SingleResponse> Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new SingleResponse(Result.BadRequest, new Metadata(Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method)), $"Parameter cannot be null : id");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method),
                Link.Edit($"{Constants.TOKENIZED_BASE_URL}/{Constants.API_ROUTE_BASE_PATH}{Constants.TOKENIZED_CONTROLLER_PATH}/{id}", HttpMethod.Put.Method),
                Link.Delete($"{Constants.TOKENIZED_BASE_URL}/{Constants.API_ROUTE_BASE_PATH}{Constants.TOKENIZED_CONTROLLER_PATH}/{id}", HttpMethod.Delete.Method),
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

        public async Task<Response> Create(PetModel pet)
        {
            if (pet == null)
            {
                return new Response(Result.BadRequest, new Metadata(Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Post.Method)), $"Parameter cannot be null : owner");
            }
            if (string.IsNullOrEmpty(pet.Id))
            {
                return new Response(Result.BadRequest, new Metadata(Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Post.Method)), $"Parameter cannot be null : owner.id");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Post.Method),
                Link.Custom("detail", $"{Constants.TOKENIZED_CURRENT_URL}/{pet.Id}/detail", HttpMethod.Get.Method),
                Link.Edit($"{Constants.TOKENIZED_CURRENT_URL}/{pet.Id}", HttpMethod.Put.Method),
                Link.Delete($"{Constants.TOKENIZED_CURRENT_URL}/{pet.Id}", HttpMethod.Delete.Method),
            });

            try
            {
                var petEntity = Mapper.Map<PetEntity>(pet);
                var added = await PetRepository.Add(petEntity);
                return new Response(Result.Created, metadata);
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

        public async Task<Response> Update(PetModel pet)
        {
            if (pet == null)
            {
                return new Response(Result.BadRequest, new Metadata(Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Put.Method)), $"Parameter cannot be null : owner");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Put.Method),
                Link.Custom("detail", $"{Constants.TOKENIZED_CURRENT_URL}/{pet.Id}/detail", HttpMethod.Get.Method),
                Link.Delete($"{Constants.TOKENIZED_CURRENT_URL}/{pet.Id}", HttpMethod.Delete.Method),
            });

            try
            {
                var petEntity = Mapper.Map<PetEntity>(pet);
                var updated = await PetRepository.Update(petEntity);
                return new Response(Result.Updated, metadata);
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

        public async Task<Response> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new Response(Result.BadRequest, new Metadata(Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Delete.Method)), $"Parameter cannot be null : id");
            }

            var metadata = new Metadata(new[]
            {
                Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Delete.Method),
            });

            try
            {
                var deleted = await PetRepository.Delete(id);
                return new Response(Result.Deleted, metadata);
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
