using System;
using Moq;
using Autofac.Extras.Moq;
using PetRego.Api;
using PetRego.Data;
using PetRego.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace PetRego.UnitTests.ServiceTests
{
    [TestClass]
    public class OwnerServiceTests
    {
        [TestInitialize]
        public void Setup()
        {
            AutomapperConfig.Configure();
        }



        [TestMethod]
        public void Can_GetAllOwners()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var expected = new List<OwnerEntity>();
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.List()).ReturnsAsync(expected);
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.GetAll().Result.Values.OfType<OwnerModel>().ToList();

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().Verify(x => x.List());
                Assert.IsNotNull(actual);
            }
        }

        [TestMethod]
        public void Can_GetOwnerById()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var ownerId = Guid.NewGuid().ToString();
                var expectedEntity = new OwnerEntity { Id = ownerId, EmailAddress = "test@test.com" };
                var expectedModel = Mapper.Map<OwnerEntity, OwnerModel>(expectedEntity);
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.Get(expectedEntity.Id)).ReturnsAsync(expectedEntity);
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.GetById(ownerId).Result.Value as OwnerModel;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().Verify(x => x.Get(ownerId));
                Assert.IsNotNull(actual);
                Assert.AreEqual(expectedModel.Id, actual.Id);
                Assert.AreEqual(expectedModel.EmailAddress, actual.EmailAddress);

            }
        }



    }
}
