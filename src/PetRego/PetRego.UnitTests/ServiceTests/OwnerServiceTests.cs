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
                var testOwner = GetTestOwner();
                var expectedOwners = new List<OwnerEntity> { testOwner };
                var expected = new MultiResponse(expectedOwners.Select(Mapper.Map<OwnerEntity, OwnerModel>).OfType<IModel>().ToList(), Result.Success);
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.List()).ReturnsAsync(expectedOwners);
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.GetAll().Result;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.Failed);
                Assert.IsNotNull(actual.Values);
                Assert.AreEqual(expected.Values.Count, actual.Values.Count);
                Assert.AreEqual(actual.Result, Result.Success);
            }
        }

        [TestMethod]
        public void Can_GetOwnerById()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var testOwner = GetTestOwner();
                var expectedOwner = Mapper.Map<OwnerEntity, OwnerModel>(testOwner);
                var expected = new SingleResponse(expectedOwner, Result.Success);
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.Get(testOwner.Id)).ReturnsAsync(testOwner);
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.GetById(testOwner.Id).Result;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.Failed);
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.Success);
                Assert.IsNotNull(actual.Value);
                var actualOwner = actual.Value as OwnerModel;
                Assert.IsNotNull(actualOwner);
                Assert.AreEqual(expectedOwner.Id, actualOwner.Id);
                Assert.AreEqual(expectedOwner.EmailAddress, actualOwner.EmailAddress);
                Assert.AreEqual(expectedOwner.FirstName, actualOwner.FirstName);
                Assert.AreEqual(expectedOwner.LastName, actualOwner.LastName);
            }
        }

        [TestMethod]
        public void Can_GetOwnerById_HandlesArgumentException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var testOwner = GetTestOwner();
                var expectedOwner = Mapper.Map<OwnerEntity, OwnerModel>(testOwner);
                var expected = new SingleResponse(expectedOwner, Result.Error);
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.Get(null)).Throws<ArgumentNullException>();
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.GetById(null).Result;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().VerifyAll(); 
                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.Failed);
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.Error);
                Assert.IsNull(actual.Value);
            }
        }

        [TestMethod]
        public void Can_GetOwnerById_HandlesDataException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var testOwner = GetTestOwner();
                var expectedOwner = Mapper.Map<OwnerEntity, OwnerModel>(testOwner);
                var expected = new SingleResponse(expectedOwner, Result.Unsupported);
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.Get(testOwner.Id)).Throws(new DataException<OwnerEntity>("test", Result.Unsupported, "error"));
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.GetById(testOwner.Id).Result;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.Failed);
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.Unsupported);
                Assert.IsNull(actual.Value);
            }
        }

        [TestMethod]
        public void Can_CreateOwner()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var testOwner = GetTestOwner();
                var expectedOwner = Mapper.Map<OwnerEntity, OwnerModel>(testOwner);
                var expected = new BasicResponse(Result.Created);
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.Add(It.IsAny<OwnerEntity>())).ReturnsAsync(true);
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.Create(expectedOwner).Result;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.Failed);
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.Created);
            }
        }

        [TestMethod]
        public void Can_UpdateOwner()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var testOwner = GetTestOwner();
                var expectedOwner = Mapper.Map<OwnerEntity, OwnerModel>(testOwner);
                var expected = new BasicResponse(Result.Updated);
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.Update(It.IsAny<OwnerEntity>())).ReturnsAsync(true);
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.Update(expectedOwner).Result;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.Failed);
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.Updated);
            }
        }


        [TestMethod]
        public void Can_DeleteOwner()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var testOwner = GetTestOwner();
                var expected = new BasicResponse(Result.Deleted);
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.Delete(testOwner.Id)).ReturnsAsync(true);
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.Delete(testOwner.Id).Result;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsFalse(actual.Failed);
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.Deleted);

            }
        }



        OwnerEntity GetTestOwner()
        {
            return new OwnerEntity
            {
                Id = Guid.NewGuid().ToString(),
                EmailAddress = "test@test.com",
                FirstName = "Test",
                LastName = "McTester"
            };
        }


    }


}
