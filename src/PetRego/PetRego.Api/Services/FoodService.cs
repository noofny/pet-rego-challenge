using System;
using System.Threading.Tasks;
using PetRego.Data;
using PetRego.Models;
using PetRego.Common;
using PetRego.Common.Extensions;
using System.Net.Http;
using AutoMapper;

namespace PetRego.Api
{
    public class FoodService
    {
        readonly IRepository<PetEntity> PetRepository;
        readonly IAppConfig AppConfig;

        public FoodService(IRepository<PetEntity> petRepository, IAppConfig appConfig)
        {
            PetRepository = petRepository;
            AppConfig = appConfig;
        }

        public async Task<SingleResponse> Summary()
        {
            var metadata = new Metadata(new[]
            {
                Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method),
            });
            try
            {
                var petEntityCount = await PetRepository.Count<PetType>("Type");
                if (petEntityCount == null)
                {
                    return new SingleResponse(Result.NotFound, new Metadata(Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method)), $"Could not find any data!");
                }
                var foodSummaryModel = new FoodSummaryModel();
                foreach(var value in petEntityCount.Values)
                {
                    var foodType = FoodType.Undefined;
                    if (string.IsNullOrEmpty(value.Key))
                    {
                        foodType = FoodType.All;
                    }
                    else
                    {
                        foodType = Mapper.Map<FoodType>(value.Key.ToEnum<PetType>());
                    }
                    foodSummaryModel.FoodCounts.Add(new FoodCount(foodType, value.Value));
                }

                return new SingleResponse(Result.Success, metadata, foodSummaryModel);
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


    }
}
