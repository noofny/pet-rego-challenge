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

        public Metadata(params Link[] links)
        {
            if (links == null || links.Length < 1)
            {
                throw new ArgumentException("One or more values must be provided!", nameof(links));
            }
            Links = links.ToList();
            ServerVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }


}
