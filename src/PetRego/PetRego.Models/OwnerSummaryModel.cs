using System;

namespace PetRego.Models
{
    public class OwnerSummaryModel : IModel
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NumberOfPets { get; set; }
        public int TotalPurchases { get; set; }
        public DateTime? LastPurchaseDate { get; set; }

    }
}
