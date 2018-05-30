using System;
using PetRego.Models;

namespace PetRego.Data
{
    public class OwnerEntity : IEntity
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Address Address { get; set; }

        public OwnerEntity()
        {
            Id = Guid.NewGuid().ToString();
        }

    }

}
