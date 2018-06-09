using System;
using PetRego.Common;

namespace PetRego.UnitTests
{
    public class TestConfig : IAppConfig
    {
        public string ElasticSearchUri { get; set; }
        public string TokenizedBaseUrl { get; set; }
        public string TokenizedCurrentUrl { get; set; }
        public string TokenizedControllerPath { get; set; }
        public string TokenizedActionPath { get; set; }

        public TestConfig()
        {
            ElasticSearchUri = "http://lcoalhost/doesnt-matter";
        }
    }

}
