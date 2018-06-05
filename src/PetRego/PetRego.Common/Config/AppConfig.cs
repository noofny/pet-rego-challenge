using System;

namespace PetRego.Common
{
    public class AppConfig : IAppConfig
    {
        public string ElasticSearchUri { get; set; }
        public string BaseUrlPath { get; set; }
        public string TokenizedBaseUrl { get; set; }

    }

}
