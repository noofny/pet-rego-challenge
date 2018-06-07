using System;

namespace PetRego.Data
{
    public interface IEntity
    {
        string Id { get; set; }
        DateTime Created { get; set; }
        DateTime? Updated { get; set; }
    }
}
