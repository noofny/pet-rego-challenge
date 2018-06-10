using PetRego.Data;
using PetRego.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PetRego.UnitTests.ServiceTests
{
    public abstract class ServiceTestsBase
    {
        [TestInitialize]
        public void Setup()
        {
            AppHost.AutomapperConfig.Configure();
        }



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
            foreach(var expectedItem in expected)
            {
                var actualItem = actual.SingleOrDefault(x => 
                    x.Action == expectedItem.Action &&
                    x.Href == expectedItem.Href &&
                    x.Rel == expectedItem.Rel
                );
                if (actualItem == null)
                {
                    return false;
                }
            }
            return true;
        }


    }


}
