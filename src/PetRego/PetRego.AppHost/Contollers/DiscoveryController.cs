using Microsoft.AspNetCore.Mvc;
using PetRego.Common;
using PetRego.Models;
using PetRego.Api;

namespace PetRego.AppHost
{
    public class DiscoveryController : ApiController
    {
        readonly DiscoveryService DiscoveryService;

        public DiscoveryController(DiscoveryService discoveryService, IAppConfig appConfig) : base(appConfig)
        {
            DiscoveryService = discoveryService;
        }

        [HttpGet]
        [Route(Service.API_ROUTE_BASE_PATH)]
        public IResponse Get()
        {
            var response = DiscoveryService.Discover();
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

    }
}
