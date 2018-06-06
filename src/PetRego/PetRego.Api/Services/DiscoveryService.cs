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
            var metadata = new Metadata(new []
            {
                Link.Self($"{AppConfig.TokenizedBaseUrl}", HttpMethod.Get.Method),

                Link.Custom("owners", $"{AppConfig.TokenizedBaseUrl}/owner/search/{{field}}/{{value}}", HttpMethod.Get.Method),
                Link.Custom("owners-by-email", $"{AppConfig.TokenizedBaseUrl}/owner/search/emailAddress/{{value}}", HttpMethod.Get.Method),
                Link.Custom("owners-by-first-name", $"{AppConfig.TokenizedBaseUrl}/owner/search/firstName/{{value}}", HttpMethod.Get.Method),
                Link.Custom("owners-by-last-name", $"{AppConfig.TokenizedBaseUrl}/owner/search/lastName/{{value}}", HttpMethod.Get.Method),
                Link.Custom("owners-by-phone", $"{AppConfig.TokenizedBaseUrl}/owner/search/phoneNumber/{{value}}", HttpMethod.Get.Method),

                Link.Custom("pets", $"{AppConfig.TokenizedBaseUrl}/pet/search/{{field}}/{{value}}", HttpMethod.Get.Method),
                Link.Custom("pets-by-name", $"{AppConfig.TokenizedBaseUrl}/pet/search/name/{{value}}", HttpMethod.Get.Method),
                Link.Custom("pets-by-type", $"{AppConfig.TokenizedBaseUrl}/pet/search/type/{{value}}", HttpMethod.Get.Method),
                Link.Custom("pets-by-breed", $"{AppConfig.TokenizedBaseUrl}/pet/search/breed/{{value}}", HttpMethod.Get.Method),
          
            });

            var response = new Response(Result.Success, metadata);
            return response;
        }


    }
}
