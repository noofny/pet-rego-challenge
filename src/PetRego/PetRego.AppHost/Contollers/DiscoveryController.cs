using Microsoft.AspNetCore.Mvc;
using PetRego.Common;
using PetRego.Models;
using PetRego.Api;

namespace PetRego.AppHost
{
    public class DiscoveryController : ApiController
    {
        readonly DiscoveryService DiscoveryService;

        public DiscoveryController(IAppConfig appConfig, DiscoveryService discoveryService) : base(appConfig)
        {
            DiscoveryService = discoveryService;
        }

        [HttpGet]
        [Route("api")]
        public IResponse Get()
        {
            var response = DiscoveryService.Discover();
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

    }
}
