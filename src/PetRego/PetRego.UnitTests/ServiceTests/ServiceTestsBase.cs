using PetRego.Data;
using PetRego.Models;
using System.Linq;
using System;
using System.Collections.Generic;

namespace PetRego.UnitTests.ServiceTests
{
    public abstract class ServiceTestsBase
    {
        protected OwnerEntity GetTestOwner()
        {
            return new OwnerEntity
            {
                EmailAddress = "test@test.com",
                FirstName = "Test",
                LastName = "McTester"
            };
        }

        protected PetEntity GetTestPet()
        {
            return new PetEntity
            {
                OwnerId = Guid.NewGuid().ToString(),
                Type = PetType.Cat,
                Name = "Mina",
                Breed = "Ginger",
                Description = "Petite, short hair, very sweet"
            };
        }

        protected bool MetadataLinksAreEqual(List<Link> expected, List<Link> actual)
        {
            if (expected == null && actual != null)
            {
                return false;
            }
            if (expected != null && actual == null)
            {
                return false;
            }
            if (expected.Count != actual.Count)
            {
                return false;
            }
            foreach(var expectedLink in expected)
            {
                var actualLink = actual.Single(x => 
                    x.Action == expectedLink.Action &&
                    x.Href == expectedLink.Href &&
                    x.Rel == expectedLink.Rel
                );
                if (actualLink == null)
                {
                    return false;
                }
            }
            return true;
        }


    }


}
