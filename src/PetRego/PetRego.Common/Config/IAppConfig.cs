using System;

namespace PetRego.Common
{
    public interface IAppConfig
    {
        string ElasticSearchUri { get; set; }
        string TokenizedBaseUrl { get; set; }

    }
}
