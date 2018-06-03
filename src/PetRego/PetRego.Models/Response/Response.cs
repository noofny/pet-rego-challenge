using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace PetRego.Models
{
    public abstract class Response 
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Result Result { get; private set; }
        public List<Link> Links { get; private set; }
        public string Error { get; private set; }
        public bool Failed => !string.IsNullOrEmpty(Error);

        protected Response(Result result, params Link[] links)
        {
            Result = result;
            Links = links.ToList();
        }
        protected Response(string error, Result result, params Link[] links)
        {
            if (string.IsNullOrEmpty(error))
            {
                throw new ArgumentNullException(nameof(error), "Error must be provided in this overload!");
            }
            Error = error;
            Result = result;
            Links = links.ToList();
        }
    }


}
