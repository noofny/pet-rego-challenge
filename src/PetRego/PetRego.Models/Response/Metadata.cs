using System;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace PetRego.Models
{
    public class Metadata
    {
        public List<Link> Links { get; }
        public string ServerVersion { get; }

        public Metadata(List<Link> links, string baseUrl)
        {
            if (links == null || links.Count < 1)
            {
                throw new ArgumentException("One or more values must be provided!", nameof(links));
            }
            Links = links;
            ServerVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }


}
