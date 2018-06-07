using System;

namespace PetRego.Models
{
    public class OwnerDetailModel : IModel
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfPets { get; set; }
        public int TotalPurchases { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public Address Address { get; set; }

    }
}
