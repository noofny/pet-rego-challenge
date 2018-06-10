using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetRego.Common;
using PetRego.Models;
using PetRego.Api;

namespace PetRego.AppHost
{
    public class FoodController : ApiController
    {
        readonly FoodService FoodService;

        public FoodController(FoodService foodService, IAppConfig appConfig): base(appConfig)
        {
            FoodService = foodService;
        }


        [HttpGet]
        [Route(Constants.API_ROUTE_BASE_PATH + Constants.API_ROUTE_CONTROLLER_PATH + "summary")]
        public async Task<IResponse> Summary()
        {
            var response = await FoodService.Summary();
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }


    }
}
