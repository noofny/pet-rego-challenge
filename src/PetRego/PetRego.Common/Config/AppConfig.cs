namespace PetRego.Common
{
    public class AppConfig : IAppConfig
    {
        public string ElasticSearchUri { get; set; }
        public string TokenizedBaseUrl { get; set; }
        public string TokenizedCurrentUrl { get; set; }
        public string TokenizedControllerPath { get; set; }
        public string TokenizedActionPath { get; set; }
    }

}
