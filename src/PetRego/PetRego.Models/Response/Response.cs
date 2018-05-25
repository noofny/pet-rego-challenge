using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PetRego.Models
{
    public abstract class Response 
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Result Result { get; private set; }
        public string Error { get; private set; }
        public bool Failed => !string.IsNullOrEmpty(Error);

        protected Response(Result result)
        {
            Result = result;
        }
        protected Response(string error, Result result)
        {
            if (string.IsNullOrEmpty(error))
            {
                throw new ArgumentNullException(nameof(error), "Error must be provided in this overload!");
            }
            Error = error;
            Result = result;
        }
    }


}
