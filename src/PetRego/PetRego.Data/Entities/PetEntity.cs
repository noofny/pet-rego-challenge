using System;

namespace PetRego.Data
{
    /// <summary>
    /// 
    /// Some Notes here;
    /// 
    /// I have made an assumption for simplicity that pets are registered to a single owner. 
    /// To support multiple owners for the same pet, a simple mapping/bridging needs to be 
    /// setup in the data repository layer and this value changed to be array (OwnerIds). 
    /// 
    /// Pet Type could be an Enum - this would make it nicer to work woth in code (strongly typed)
    /// however without capturing every possible value, a future change requires the customer
    /// to pay for a code change made and a re-deployment, which may not be good for them.
    /// 
    /// </summary>
    public class PetEntity : IEntity
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
