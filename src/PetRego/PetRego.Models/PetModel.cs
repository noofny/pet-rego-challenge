using System;

namespace PetRego.Models
{
    public class PetModel : IModel
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string Type { get; set; }
        public string Breed { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }
}
