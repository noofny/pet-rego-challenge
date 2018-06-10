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

        public Response Discover()
        {
            var metadata = new Metadata(new []
            {
                Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method),

                Link.Custom("owners-search", $"{Constants.TOKENIZED_CURRENT_URL}/owner/search/{{field}}/{{value}}", HttpMethod.Get.Method),
                Link.Custom("owners-by-email", $"{Constants.TOKENIZED_CURRENT_URL}/owner/search/emailAddress/{{value}}", HttpMethod.Get.Method),
                Link.Custom("owners-by-first-name", $"{Constants.TOKENIZED_CURRENT_URL}/owner/search/firstName/{{value}}", HttpMethod.Get.Method),
                Link.Custom("owners-by-last-name", $"{Constants.TOKENIZED_CURRENT_URL}/owner/search/lastName/{{value}}", HttpMethod.Get.Method),
                Link.Custom("owners-by-phone", $"{Constants.TOKENIZED_CURRENT_URL}/owner/search/phoneNumber/{{value}}", HttpMethod.Get.Method),

                Link.Custom("pets-search", $"{Constants.TOKENIZED_CURRENT_URL}/pet/search/{{field}}/{{value}}", HttpMethod.Get.Method),
                Link.Custom("pets-by-name", $"{Constants.TOKENIZED_CURRENT_URL}/pet/search/name/{{value}}", HttpMethod.Get.Method),
                Link.Custom("pets-by-type", $"{Constants.TOKENIZED_CURRENT_URL}/pet/search/type/{{value}}", HttpMethod.Get.Method),
                Link.Custom("pets-by-breed", $"{Constants.TOKENIZED_CURRENT_URL}/pet/search/breed/{{value}}", HttpMethod.Get.Method),

                Link.Custom("food-summary", $"{Constants.TOKENIZED_CURRENT_URL}/food/summary", HttpMethod.Get.Method),

            });

            var response = new Response(Result.Success, metadata);
            return response;
        }


    }
}
