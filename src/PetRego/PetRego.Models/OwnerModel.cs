using System;

namespace PetRego.Models
{
    public class OwnerModel : IModel
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Address Address { get; set; }

    }
}
