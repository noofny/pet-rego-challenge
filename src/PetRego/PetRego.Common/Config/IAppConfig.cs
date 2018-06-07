namespace PetRego.Common
{
    public interface IAppConfig
    {
        string ElasticSearchUri { get; set; }
        string TokenizedBaseUrl { get; set; }
        string TokenizedCurrentUrl { get; set; }
        string TokenizedControllerPath { get; set; }
        string TokenizedActionPath { get; set; }

    }
}
