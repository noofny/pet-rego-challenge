﻿using System;

namespace PetRego.Common
{
    public interface IAppConfig
    {
        string ElasticSearchUri { get; set; }
        string BaseUrlPath { get; set; }
        string TokenizedBaseUrl { get; set; }

    }
}
