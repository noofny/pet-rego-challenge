using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace PetRego.Models
{
    public abstract class HypermediaResponse
    {
        readonly List<Link> _links = new List<Link>();

        [JsonProperty(Order = 100)]
        public List<Link> Links => _links; 

        public Result Result { get; private set; }

        protected HypermediaResponse(Result result)
        {
            Result = result;
        }

        public void AddLink(Link link)
        {
            _links.Add(link);
        }

        public void AddLinks(params Link[] links)
        {
            _links.AddRange(links);
        }


    }


}
