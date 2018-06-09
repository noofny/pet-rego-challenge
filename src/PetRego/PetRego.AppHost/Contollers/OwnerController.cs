using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetRego.Common;
using PetRego.Models;
using PetRego.Api;

namespace PetRego.AppHost
{
    public class OwnerController : ApiController
    {
        readonly OwnerService OwnerService;

        public OwnerController(OwnerService ownerService, IAppConfig appConfig): base(appConfig)
        {
            OwnerService = ownerService;
        }


        [HttpGet]
        [Route(Constants.API_ROUTE_BASE_PATH + Constants.API_ROUTE_CONTROLLER_PATH + "search/{field}/{value}")]
        public async Task<IResponse> Search(string field, string value)
        {
            var response = await OwnerService.Search(field, value);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpGet]
        [Route(Constants.API_ROUTE_BASE_PATH + Constants.API_ROUTE_CONTROLLER_PATH + "{id}/summary")]
        public async Task<IResponse> Summary(string id)
        {
            var response = await OwnerService.Summary(id);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpGet]
        [Route(Constants.API_ROUTE_BASE_PATH + Constants.API_ROUTE_CONTROLLER_PATH + "{id}/detail")]
        public async Task<IResponse> Detail(string id)
        {
            var response = await OwnerService.Detail(id);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpPost]
        [Route(Constants.API_ROUTE_BASE_PATH + Constants.API_ROUTE_CONTROLLER_PATH)]
        public async Task<IResponse> Create([FromBody]OwnerDetailModel owner)
        {
            var response = await OwnerService.Create(owner);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpPut]
        [Route(Constants.API_ROUTE_BASE_PATH + Constants.API_ROUTE_CONTROLLER_PATH + "{id}")]
        public async Task<IResponse> Edit(string id, [FromBody]OwnerDetailModel owner)
        {
            owner.Id = id;
            var response = await OwnerService.Update(owner);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpDelete]
        [Route(Constants.API_ROUTE_BASE_PATH + Constants.API_ROUTE_CONTROLLER_PATH + "{id}")]
        public async Task<IResponse> Delete(string id)
        {
            var response = await OwnerService.Delete(id);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }


    }
}
