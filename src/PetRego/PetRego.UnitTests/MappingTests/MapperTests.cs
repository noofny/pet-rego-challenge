using System;
using Moq;
using Autofac;
using Autofac.Extras.Moq;
using PetRego.Api;
using PetRego.Data;
using PetRego.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;

namespace PetRego.UnitTests.MappingTests
{
    [TestClass]
    public class MapperTests
    {
        [TestInitialize]
        public void Setup()
        {
            AutomapperConfig.Configure();
        }

        [TestMethod]
        public void Can_Map_OwnerEntity_To_OwnerModel()
        {
            var rnd = new Random();
            var source = new OwnerEntity
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Mary",
                LastName = "Swivellnail",
                EmailAddress = "mary.swivellnail@gmail.com",
                DateOfBirth = DateTime.Now.Subtract(TimeSpan.FromDays(rnd.Next(1,9999))),
                Address = new Address
                {
                    City = "Sydney",
                    Country = "Australia",
                    PostCode = "2000",
                    State = "NSW",
                    StreetName = "York St",
                    StreetNumber = "1"
                }
            };
            var target = Mapper.Map<OwnerEntity, OwnerModel>(source);

            Assert.IsNotNull(target);
            Assert.AreEqual(source.Id, target.Id);
            Assert.AreEqual(source.FirstName, target.FirstName);
            Assert.AreEqual(source.LastName, target.LastName);
            Assert.AreEqual(source.EmailAddress, target.EmailAddress);
            Assert.AreEqual(source.DateOfBirth, target.DateOfBirth);
            Assert.IsNotNull(target.Address);
            Assert.AreEqual(source.Address.City, target.Address.City);
            Assert.AreEqual(source.Address.Country, target.Address.Country);
            Assert.AreEqual(source.Address.PostCode, target.Address.PostCode);
            Assert.AreEqual(source.Address.State, target.Address.State);
            Assert.AreEqual(source.Address.StreetName, target.Address.StreetName);
            Assert.AreEqual(source.Address.StreetNumber, target.Address.StreetNumber);

        }

        [TestMethod]
        public void Can_Map_PetEntity_To_PetModel()
        {
            var rnd = new Random();
            var source = new PetEntity
            {
                Id = Guid.NewGuid().ToString(),
                Breed = "Bengal",
                Description = "Grey with blue spots",
                Name = "Jubjub",
                OwnerId = Guid.NewGuid().ToString(),
                Type = "Cat",
                DateOfBirth = DateTime.Now.Subtract(TimeSpan.FromDays(rnd.Next(1, 9999))),
            };
            var target = Mapper.Map<PetEntity, PetModel>(source);

            Assert.IsNotNull(target);
            Assert.AreEqual(source.Id, target.Id);
            Assert.AreEqual(source.Breed, target.Breed);
            Assert.AreEqual(source.Description, target.Description);
            Assert.AreEqual(source.Name, target.Name);
            Assert.AreEqual(source.OwnerId, target.OwnerId);
            Assert.AreEqual(source.Type, target.Type);
            Assert.AreEqual(source.DateOfBirth, target.DateOfBirth);
        }









        [TestMethod]
        public void Can_Map_OwnerModel_To_OwnerEntity()
        {
            var rnd = new Random();
            var source = new OwnerModel
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Mary",
                LastName = "Swivellnail",
                EmailAddress = "mary.swivellnail@gmail.com",
                DateOfBirth = DateTime.Now.Subtract(TimeSpan.FromDays(rnd.Next(1, 9999))),
                Address = new Address
                {
                    City = "Sydney",
                    Country = "Australia",
                    PostCode = "2000",
                    State = "NSW",
                    StreetName = "York St",
                    StreetNumber = "1"
                }
            };
            var target = Mapper.Map<OwnerModel, OwnerEntity>(source);

            Assert.IsNotNull(target);
            Assert.AreEqual(source.Id, target.Id);
            Assert.AreEqual(source.FirstName, target.FirstName);
            Assert.AreEqual(source.LastName, target.LastName);
            Assert.AreEqual(source.EmailAddress, target.EmailAddress);
            Assert.AreEqual(source.DateOfBirth, target.DateOfBirth);
            Assert.IsNotNull(target.Address);
            Assert.AreEqual(source.Address.City, target.Address.City);
            Assert.AreEqual(source.Address.Country, target.Address.Country);
            Assert.AreEqual(source.Address.PostCode, target.Address.PostCode);
            Assert.AreEqual(source.Address.State, target.Address.State);
            Assert.AreEqual(source.Address.StreetName, target.Address.StreetName);
            Assert.AreEqual(source.Address.StreetNumber, target.Address.StreetNumber);

        }

        [TestMethod]
        public void Can_Map_PetModel_To_PetEntity()
        {
            var rnd = new Random();
            var source = new PetModel
            {
                Id = Guid.NewGuid().ToString(),
                Breed = "Bengal",
                Description = "Grey with blue spots",
                Name = "Jubjub",
                OwnerId = Guid.NewGuid().ToString(),
                Type = "Cat",
                DateOfBirth = DateTime.Now.Subtract(TimeSpan.FromDays(rnd.Next(1, 9999))),
            };
            var target = Mapper.Map<PetModel, PetEntity>(source);

            Assert.IsNotNull(target);
            Assert.AreEqual(source.Id, target.Id);
            Assert.AreEqual(source.Breed, target.Breed);
            Assert.AreEqual(source.Description, target.Description);
            Assert.AreEqual(source.Name, target.Name);
            Assert.AreEqual(source.OwnerId, target.OwnerId);
            Assert.AreEqual(source.Type, target.Type);
            Assert.AreEqual(source.DateOfBirth, target.DateOfBirth);
        }



    }
}
