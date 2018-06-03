using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PetRego.Models
{
    public class PetModel : IModel
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PetType Type { get; set; }
        public string Breed { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }
}
