using System.Collections.Generic;
using System.Net.Http;
using PetRego.Models;
using PetRego.Common;

namespace PetRego.Api
{
    public class DiscoveryService : Service
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
                Link.Self($"{AppConfig.TokenizedCurrentUrl}", HttpMethod.Get.Method),

                Link.Custom("owners", $"{AppConfig.TokenizedCurrentUrl}/owner/search/{{field}}/{{value}}", HttpMethod.Get.Method),
                Link.Custom("owners-by-email", $"{AppConfig.TokenizedCurrentUrl}/owner/search/emailAddress/{{value}}", HttpMethod.Get.Method),
                Link.Custom("owners-by-first-name", $"{AppConfig.TokenizedCurrentUrl}/owner/search/firstName/{{value}}", HttpMethod.Get.Method),
                Link.Custom("owners-by-last-name", $"{AppConfig.TokenizedCurrentUrl}/owner/search/lastName/{{value}}", HttpMethod.Get.Method),
                Link.Custom("owners-by-phone", $"{AppConfig.TokenizedCurrentUrl}/owner/search/phoneNumber/{{value}}", HttpMethod.Get.Method),

                Link.Custom("pets", $"{AppConfig.TokenizedCurrentUrl}/pet/search/{{field}}/{{value}}", HttpMethod.Get.Method),
                Link.Custom("pets-by-name", $"{AppConfig.TokenizedCurrentUrl}/pet/search/name/{{value}}", HttpMethod.Get.Method),
                Link.Custom("pets-by-type", $"{AppConfig.TokenizedCurrentUrl}/pet/search/type/{{value}}", HttpMethod.Get.Method),
                Link.Custom("pets-by-breed", $"{AppConfig.TokenizedCurrentUrl}/pet/search/breed/{{value}}", HttpMethod.Get.Method),
          
            });

            var response = new Response(Result.Success, metadata);
            return response;
        }


    }
}
