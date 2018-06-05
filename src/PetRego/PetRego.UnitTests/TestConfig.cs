using System;

namespace PetRego.Common
{
    public class TestConfig : IAppConfig
    {
        public string ElasticSearchUri { get; set; }
        public string BaseUrlPath { get; set; }
        public string TokenizedBaseUrl { get; set; }

        public TestConfig()
        {
            ElasticSearchUri = "http://lcoalhost/doesnt-matter";
            BaseUrlPath = "api";
            TokenizedBaseUrl = "<<BASE_URL>>";

        }
    }

}
