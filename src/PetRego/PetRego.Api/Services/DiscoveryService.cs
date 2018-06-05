using System.Collections.Generic;
using System.Net.Http;
using PetRego.Models;
using PetRego.Common;

namespace PetRego.Api
{
    public class DiscoveryService 
    {
        readonly IAppConfig AppConfig;

        public DiscoveryService(IAppConfig appConfig)
        {
            AppConfig = appConfig;
        }

        public IResponse Discover()
        {
            var metadata = new Metadata(new List<Link>
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Get.Method),
                Link.Related($"{AppConfig.TokenizedBaseUrl}/owner", HttpMethod.Get.Method),
                Link.Related($"{AppConfig.TokenizedBaseUrl}/pet", HttpMethod.Get.Method),
            }, AppConfig.TokenizedBaseUrl);

            var response = new Response(Result.Success, metadata);
            return response;
        }


    }
}
