using System;
using PetRego.Models;

namespace PetRego.Data
{
    public class OwnerEntity : IEntity
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int TotalPurchases { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public Address Address { get; set; }

        public OwnerEntity()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.UtcNow;
        }

    }

}
