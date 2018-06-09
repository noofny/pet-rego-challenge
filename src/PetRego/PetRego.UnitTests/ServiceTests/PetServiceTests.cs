using System;
using Moq;
using Autofac.Extras.Moq;
using PetRego.Api;
using PetRego.Common;
using PetRego.Data;
using PetRego.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace PetRego.UnitTests.ServiceTests
{
    /// <summary>
    /// 
    /// These unit tests are testing way too much in my oppinion. 
    /// 
    /// The services are currently responsible for enriching responses with hypermedia, 
    /// instead of splitting this concern out to a seperate component. 
    /// Because of this, we have testing the internal mechanics of the services AND the
    /// structure of thr responses all in one. 
    /// 
    /// Given more time, I would improve this, perhaps by;
    ///   a) Creating another layer between controllers and services which enriches the hypermedia
    ///   b) Leverage the WebAPI response filters to inject the hypermedia at that point
    ///   c) Find a library that handles this gracefully, such as ServiceStack.
    /// 
    /// </summary>
    [TestClass]
    public class PetServiceTests : ServiceTestsBase
    {


        [TestMethod]
        public void Can_Search_Pets()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var config = mock.Provide<IAppConfig, TestConfig>();
                var searchCriteria = new { field = "name", value = "mina" };
                var testPet = GetTestPet();
                var expectedMetadata = new Metadata(new[]
                {
                    Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method),
                    Link.Custom("detail", $"{Constants.TOKENIZED_BASE_URL}/{Constants.API_ROUTE_BASE_PATH}{Constants.TOKENIZED_CONTROLLER_PATH}/{testPet.Id}/detail", HttpMethod.Get.Method),
                });
                var expectedPets = new List<PetEntity> { testPet };
                var expected = new MultiResponse(Result.Success, expectedMetadata, expectedPets.Select(Mapper.Map<PetEntity, PetModel>).OfType<IModel>().ToList());
                mock.Mock<IRepository<PetEntity>>().Setup(x => x.Search(searchCriteria.field, searchCriteria.value)).ReturnsAsync(expectedPets);
                var systemUnderTest = mock.Create<PetService>();

                // Act
                var actual = systemUnderTest.Search(searchCriteria.field, searchCriteria.value).Result;

                // Assert
                mock.Mock<IRepository<PetEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Errors);
                Assert.IsNotNull(actual.Data);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.AreEqual(actual.Result, Result.Success);
                Assert.AreEqual(expected.Data.Count, actual.Data.Count);
                // todo - compare and assert the actual data
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.IsTrue(MetadataLinksAreEqual(expected.Metadata.Links, actual.Metadata.Links));
            }
        }

        [TestMethod]
        public void Can_Get_Pet_Detail()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var config = mock.Provide<IAppConfig, TestConfig>();
                var testPet = GetTestPet();
                var expectedMetadata = new Metadata(new[]
                {
                    Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method),
                    Link.Edit($"{Constants.TOKENIZED_BASE_URL}/{Constants.API_ROUTE_BASE_PATH}{Constants.TOKENIZED_CONTROLLER_PATH}/{testPet.Id}", HttpMethod.Put.Method),
                    Link.Delete($"{Constants.TOKENIZED_BASE_URL}/{Constants.API_ROUTE_BASE_PATH}{Constants.TOKENIZED_CONTROLLER_PATH}/{testPet.Id}", HttpMethod.Delete.Method),
                });
                var expectedPet = Mapper.Map<PetEntity, PetModel>(testPet);
                var expected = new SingleResponse(Result.Success, expectedMetadata, expectedPet);
                mock.Mock<IRepository<PetEntity>>().Setup(x => x.Get(testPet.Id)).ReturnsAsync(testPet);
                var systemUnderTest = mock.Create<PetService>();

                // Act
                var actual = systemUnderTest.Detail(testPet.Id).Result;

                // Assert
                mock.Mock<IRepository<PetEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Errors);
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.Success);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.IsNotNull(actual.Data);
                var actualPet = actual.Data as PetModel;
                Assert.IsNotNull(actualPet);
                Assert.AreEqual(expectedPet.Id, actualPet.Id);
                Assert.AreEqual(expectedPet.OwnerId, actualPet.OwnerId);
                Assert.AreEqual(expectedPet.Type, actualPet.Type);
                Assert.AreEqual(expectedPet.Name, actualPet.Name);
                Assert.AreEqual(expectedPet.Description, actualPet.Description);
                Assert.AreEqual(expectedPet.Breed, actualPet.Breed);
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.IsTrue(MetadataLinksAreEqual(expected.Metadata.Links, actual.Metadata.Links));
            }
        }

        [TestMethod]
        public void Can_Get_Pet_Handles_Bad_Request()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var config = mock.Provide<IAppConfig, TestConfig>();
                var testPet = GetTestPet();
                var expectedMetadata = new Metadata(Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method));
                var expectedPet = Mapper.Map<PetEntity, PetModel>(testPet);
                var expected = new SingleResponse(Result.BadRequest, expectedMetadata, expectedPet);
                var systemUnderTest = mock.Create<PetService>();

                // Act
                var actual = systemUnderTest.Detail(null).Result;

                // Assert
                mock.Mock<IRepository<PetEntity>>().VerifyAll(); // no setups to test here but keeping for consistency
                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Errors);
                Assert.IsTrue(actual.Errors.Count > 0); // MS provide no 'IsNotEmpty'
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.BadRequest);
                Assert.IsNull(actual.Data);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.IsTrue(MetadataLinksAreEqual(expected.Metadata.Links, actual.Metadata.Links));
            }
        }

        [TestMethod]
        public void Can_Get_Pet_Handles_Server_Error()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var config = mock.Provide<IAppConfig, TestConfig>();
                var testPet = GetTestPet();
                var expectedMetadata = new Metadata(new[]
                {
                    Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method),
                    Link.Edit($"{Constants.TOKENIZED_BASE_URL}/{Constants.API_ROUTE_BASE_PATH}{Constants.TOKENIZED_CONTROLLER_PATH}/{testPet.Id}", HttpMethod.Put.Method),
                    Link.Delete($"{Constants.TOKENIZED_BASE_URL}/{Constants.API_ROUTE_BASE_PATH}{Constants.TOKENIZED_CONTROLLER_PATH}/{testPet.Id}", HttpMethod.Delete.Method),
                });
                var expectedPet = Mapper.Map<PetEntity, PetModel>(testPet);
                var expected = new SingleResponse(Result.InternalError, expectedMetadata, expectedPet);
                mock.Mock<IRepository<PetEntity>>().Setup(x => x.Get(testPet.Id)).Throws(new DataException<PetEntity>("test", Result.InternalError, "error"));
                var systemUnderTest = mock.Create<PetService>();

                // Act
                var actual = systemUnderTest.Detail(testPet.Id).Result;

                // Assert
                mock.Mock<IRepository<PetEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Errors);
                Assert.IsTrue(actual.Errors.Count > 0); // MS provide no 'IsNotEmpty'
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.InternalError);
                Assert.IsNull(actual.Data);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.IsTrue(MetadataLinksAreEqual(expected.Metadata.Links, actual.Metadata.Links));
            }
        }

        [TestMethod]
        public void Can_Create_Pet()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var config = mock.Provide<IAppConfig, TestConfig>();
                var testPet = GetTestPet();
                var expectedMetadata = new Metadata(new[]
                {
                    Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Post.Method),
                    Link.Custom("detail", $"{Constants.TOKENIZED_CURRENT_URL}/{testPet.Id}/detail", HttpMethod.Get.Method),
                    Link.Edit($"{Constants.TOKENIZED_CURRENT_URL}/{testPet.Id}", HttpMethod.Put.Method),
                    Link.Delete($"{Constants.TOKENIZED_CURRENT_URL}/{testPet.Id}", HttpMethod.Delete.Method),
                });
                var expectedPet = Mapper.Map<PetEntity, PetModel>(testPet);
                var expected = new Response(Result.Created, expectedMetadata);
                mock.Mock<IRepository<PetEntity>>().Setup(x => x.Add(It.IsAny<PetEntity>())).ReturnsAsync(true);
                var systemUnderTest = mock.Create<PetService>();

                // Act
                var actual = systemUnderTest.Create(expectedPet).Result;

                // Assert
                mock.Mock<IRepository<PetEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Errors);
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.Created);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.IsTrue(MetadataLinksAreEqual(expected.Metadata.Links, actual.Metadata.Links));
            }
        }

        [TestMethod]
        public void Can_Update_Pet()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var config = mock.Provide<IAppConfig, TestConfig>();
                var testPet = GetTestPet();
                var expectedMetadata = new Metadata(new[]
                {
                    Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Put.Method),
                    Link.Custom("detail", $"{Constants.TOKENIZED_CURRENT_URL}/{testPet.Id}/detail", HttpMethod.Get.Method),
                    Link.Delete($"{Constants.TOKENIZED_CURRENT_URL}/{testPet.Id}", HttpMethod.Delete.Method),
                });
                var expectedPet = Mapper.Map<PetEntity, PetModel>(testPet);
                var expected = new Response(Result.Updated, expectedMetadata);
                mock.Mock<IRepository<PetEntity>>().Setup(x => x.Update(It.IsAny<PetEntity>())).ReturnsAsync(true);
                var systemUnderTest = mock.Create<PetService>();

                // Act
                var actual = systemUnderTest.Update(expectedPet).Result;

                // Assert
                mock.Mock<IRepository<PetEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Errors);
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.Updated);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.IsTrue(MetadataLinksAreEqual(expected.Metadata.Links, actual.Metadata.Links));
            }
        }


        [TestMethod]
        public void Can_DeletePet()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var config = mock.Provide<IAppConfig, TestConfig>();
                var testPet = GetTestPet();
                var expectedMetadata = new Metadata(new[]
                {
                    Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Delete.Method),
                });
                var expected = new Response(Result.Deleted, expectedMetadata);
                mock.Mock<IRepository<PetEntity>>().Setup(x => x.Delete(testPet.Id)).ReturnsAsync(true);
                var systemUnderTest = mock.Create<PetService>();

                // Act
                var actual = systemUnderTest.Delete(testPet.Id).Result;

                // Assert
                mock.Mock<IRepository<PetEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Errors);
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.Deleted);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.IsTrue(MetadataLinksAreEqual(expected.Metadata.Links, actual.Metadata.Links));
            }
        }





    }


}
