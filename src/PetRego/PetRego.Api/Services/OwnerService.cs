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
        readonly IAppConfig AppConfig;

        public OwnerService(IRepository<OwnerEntity> ownerRepository, IAppConfig appConfig)
        {
            OwnerRepository = ownerRepository;
            AppConfig = appConfig;
        }

        public async Task<IResponse> GetAll()
        {
            var metadata = new Metadata(new List<Link>
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Get.Method),
                Link.Related($"{AppConfig.TokenizedBaseUrl}/{{id}}", HttpMethod.Get.Method),
            }, AppConfig.TokenizedBaseUrl);

            try
            {
                var entities = await OwnerRepository.List();
                var models = Mapper.Map<List<OwnerModel>>(entities);
                return new MultiResponse(Result.Success, metadata, models.OfType<IModel>().ToList());
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

        public async Task<IResponse> GetById(string id)
        {
            var metadata = new Metadata(new List<Link>
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}/{id}", HttpMethod.Get.Method),
                Link.Edit($"{AppConfig.TokenizedBaseUrl}/{id}", HttpMethod.Put.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}/{id}", HttpMethod.Delete.Method),
            }, AppConfig.TokenizedBaseUrl);

            if (string.IsNullOrEmpty(id))
            {
                return new SingleResponse(Result.BadRequest, metadata, $"Parameter cannot be null : id");
            }

            try
            {
                var entity = await OwnerRepository.Get(id);
                var model = Mapper.Map<OwnerModel>(entity);
                return new SingleResponse(Result.Success, metadata, model);
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

        public async Task<IResponse> Create(OwnerModel owner)
        {
            var metadata = new Metadata(new List<Link>
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Post.Method),
                Link.Related($"{AppConfig.TokenizedBaseUrl}/{owner.Id}", HttpMethod.Get.Method),
                Link.Edit($"{AppConfig.TokenizedBaseUrl}/{owner.Id}", HttpMethod.Put.Method),
                Link.Delete($"{AppConfig.TokenizedBaseUrl}/{owner.Id}", HttpMethod.Delete.Method),
            }, AppConfig.TokenizedBaseUrl);

            if (owner == null)
            {
                return new SingleResponse(Result.BadRequest, metadata, $"Parameter cannot be null : owner");
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

        public async Task<IResponse> Update(OwnerModel owner)
        {
            var metadata = new Metadata(new List<Link>
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Put.Method),
                Link.Related($"{AppConfig.TokenizedBaseUrl}/{owner.Id}", HttpMethod.Get.Method),
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
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Delete.Method),
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
