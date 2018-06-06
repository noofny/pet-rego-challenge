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
                Link.Custom("owners", $"{AppConfig.TokenizedBaseUrl}/owner/search/{{emailAddress}}", HttpMethod.Get.Method),
                Link.Custom("pets", $"{AppConfig.TokenizedBaseUrl}/pet/search/{{name}}", HttpMethod.Get.Method),
            }, AppConfig.TokenizedBaseUrl);

            var response = new Response(Result.Success, metadata);
            return response;
        }


    }
}
