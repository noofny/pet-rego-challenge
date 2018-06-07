using System;
using PetRego.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PetRego.Data
{
    /// <summary>
    ///  
    /// I made an assumption here to keep things simple: pets are registered to a single owner. 
    /// To support multiple owners for the same pet, I would set this mapping up in the data layer, 
    /// then the OwnerId field would be changed to an array and named OwnerIds. 
    /// 
    /// </summary>
    public class PetEntity : IEntity
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string OwnerId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PetType Type { get; set; }
        public string Breed { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public PetEntity()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.UtcNow;
        }

    }
}
