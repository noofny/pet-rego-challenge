using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetRego.Data;
using PetRego.Models;
using AutoMapper;

namespace PetRego.Api
{
    public class OwnerService 
    {
        readonly IRepository<OwnerEntity> OwnerRepository;

        public OwnerService(IRepository<OwnerEntity> ownerRepository)
        {
            OwnerRepository = ownerRepository;
        }

        public async Task<MultiResponse> GetAll()
        {
            try
            {
                var entities = await OwnerRepository.List();
                var models = Mapper.Map<List<OwnerModel>>(entities);
                return new MultiResponse(models.OfType<IModel>().ToList(), Result.Success);
            }
            catch(DataException<IEntity> ex)
            {
                return new MultiResponse($"{ex.Message}", ex.Result);
            }
            catch (Exception ex)
            {
                return new MultiResponse($"{ex.GetBaseException().Message}", Result.Error);
            }
        }

        public async Task<SingleResponse> GetById(string id)
        {
            try
            {
                var entity = await OwnerRepository.Get(id);
                var model = Mapper.Map<OwnerModel>(entity);
                return new SingleResponse(model, Result.Success);
            }
            catch (DataException<IEntity> ex)
            {
                return new SingleResponse($"{ex.Message}", ex.Result);
            }
            catch (Exception ex)
            {
                return new SingleResponse($"{ex.GetBaseException().Message}", Result.Error);
            }
        }

        public async Task<BasicResponse> Create(OwnerModel owner)
        {
            try
            {
                var entity = Mapper.Map<OwnerEntity>(owner);
                await OwnerRepository.Add(entity);
                return new BasicResponse(Result.Created);
            }
            catch (DataException<IEntity> ex)
            {
                return new BasicResponse($"{ex.Message}", ex.Result);
            }
            catch (Exception ex)
            {
                return new BasicResponse($"{ex.GetBaseException().Message}", Result.Error);
            }
        }

        public async Task<BasicResponse> Update(OwnerModel owner)
        {
            try
            {
                var entity = Mapper.Map<OwnerEntity>(owner);
                await OwnerRepository.Update(entity);
                return new BasicResponse(Result.Updated);
            }
            catch (DataException<IEntity> ex)
            {
                return new BasicResponse($"{ex.Message}", ex.Result);
            }
            catch (Exception ex)
            {
                return new BasicResponse($"{ex.GetBaseException().Message}", Result.Error);
            }
        }

        public async Task<BasicResponse> Delete(string id)
        {
            try
            {
                await OwnerRepository.Delete(id);
                return new BasicResponse(Result.Deleted);
            }
            catch (DataException<IEntity> ex)
            {
                return new BasicResponse($"{ex.Message}", ex.Result);
            }
            catch (Exception ex)
            {
                return new BasicResponse($"{ex.GetBaseException().Message}", Result.Error);
            }
        }

    }
}
