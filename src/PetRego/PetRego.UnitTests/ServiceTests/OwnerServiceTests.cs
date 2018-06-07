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
    public class OwnerServiceTests
    {

        [TestInitialize]
        public void Setup()
        {
            AutomapperConfig.Configure();
        }



        [TestMethod]
        public void Can_Search_Owners()
        {
            var config = new TestConfig(); // todo - pull from container
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var searchCriteria = new { field = "emailAddress", value = "test" };
                var testOwner = GetTestOwner();
                var expectedMetadata = new Metadata(new[]
                {
                    Link.Self($"{config.TokenizedBaseUrl}", HttpMethod.Get.Method),
                    Link.Custom("summary", $"{config.TokenizedBaseUrl}/{{id}}/summary", HttpMethod.Get.Method),
                    Link.Custom("detail", $"{config.TokenizedBaseUrl}/{{id}}/detail", HttpMethod.Get.Method),
                });
                var expectedOwners = new List<OwnerEntity> { testOwner };
                var expected = new MultiResponse(Result.Success, expectedMetadata, expectedOwners.Select(Mapper.Map<OwnerEntity, OwnerSummaryModel>).OfType<IModel>().ToList());
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.Search(searchCriteria.field, searchCriteria.value)).ReturnsAsync(expectedOwners);
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.Search(searchCriteria.field, searchCriteria.value).Result;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Errors);
                Assert.IsNotNull(actual.Data);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.AreEqual(actual.Result, Result.Success);
                Assert.AreEqual(expected.Data.Count, actual.Data.Count);
                // todo - compare and assert the actual data
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.AreEqual(expected.Metadata.Links.Count, actual.Metadata.Links.Count);
                // todo - compare and assert the actual links
            }
        }

        [TestMethod]
        public void Can_Get_Owner_Summary()
        {
            var config = new TestConfig(); // todo - pull from container
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var testOwner = GetTestOwner();
                var expectedMetadata = new Metadata(new[]
                {
                    Link.Self($"{config.TokenizedBaseUrl}", HttpMethod.Get.Method),
                    Link.Edit($"{config.TokenizedBaseUrl}", HttpMethod.Put.Method),
                    Link.Delete($"{config.TokenizedBaseUrl}", HttpMethod.Delete.Method),
                    Link.Custom("detail", $"{config.TokenizedBaseUrl}/detail", HttpMethod.Get.Method),
                    Link.Custom("pets", $"{config.TokenizedBaseUrl}/pets", HttpMethod.Get.Method),
                });
                var expectedOwner = Mapper.Map<OwnerEntity, OwnerSummaryModel>(testOwner);
                var expected = new SingleResponse(Result.Success, expectedMetadata, expectedOwner);
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.Get(testOwner.Id)).ReturnsAsync(testOwner);
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.Summary(testOwner.Id).Result;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Errors);
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.Success);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.IsNotNull(actual.Data);
                var actualOwner = actual.Data as OwnerSummaryModel;
                Assert.IsNotNull(actualOwner);
                Assert.AreEqual(expectedOwner.Id, actualOwner.Id);
                Assert.AreEqual(expectedOwner.EmailAddress, actualOwner.EmailAddress);
                Assert.AreEqual(expectedOwner.FirstName, actualOwner.FirstName);
                Assert.AreEqual(expectedOwner.LastName, actualOwner.LastName);
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.AreEqual(expected.Metadata.Links.Count, actual.Metadata.Links.Count);
                // todo - compare and assert the actual links
            }
        }

        [TestMethod]
        public void Can_Get_Owner_Summary_Handles_Bad_Request()
        {
            var config = new TestConfig(); // todo - pull from container
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var testOwner = GetTestOwner();
                var expectedMetadata = new Metadata(Link.Self($"{config.TokenizedBaseUrl}", HttpMethod.Get.Method));
                var expectedOwner = Mapper.Map<OwnerEntity, OwnerSummaryModel>(testOwner);
                var expected = new SingleResponse(Result.BadRequest, expectedMetadata, expectedOwner);
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.Summary(null).Result;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().VerifyAll(); // no setups to test here but keeping for consistency
                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Errors);
                Assert.IsTrue(actual.Errors.Count > 0); // MS provide no 'IsNotEmpty'
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.BadRequest);
                Assert.IsNull(actual.Data);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.AreEqual(expected.Metadata.Links.Count, actual.Metadata.Links.Count);
                // todo - compare and assert the actual links
            }
        }

        [TestMethod]
        public void Can_Get_Owner_Summary_Handles_Server_Error()
        {
        var config = new TestConfig(); // todo - pull from container
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var testOwner = GetTestOwner();
                var expectedMetadata = new Metadata(new[]
                {
                    Link.Self($"{config.TokenizedBaseUrl}", HttpMethod.Get.Method),
                    Link.Edit($"{config.TokenizedBaseUrl}", HttpMethod.Put.Method),
                    Link.Delete($"{config.TokenizedBaseUrl}", HttpMethod.Delete.Method),
                    Link.Custom("detail", $"{config.TokenizedBaseUrl}/detail", HttpMethod.Get.Method),
                    Link.Custom("pets", $"{config.TokenizedBaseUrl}/pets", HttpMethod.Get.Method),
                });
                var expectedOwner = Mapper.Map<OwnerEntity, OwnerSummaryModel>(testOwner);
                var expected = new SingleResponse(Result.InternalError, expectedMetadata, expectedOwner);
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.Get(testOwner.Id)).Throws(new DataException<OwnerEntity>("test", Result.InternalError, "error"));
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.Summary(testOwner.Id).Result;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Errors);
                Assert.IsTrue(actual.Errors.Count > 0); // MS provide no 'IsNotEmpty'
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.InternalError);
                Assert.IsNull(actual.Data);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.AreEqual(expected.Metadata.Links.Count, actual.Metadata.Links.Count);
                // todo - compare and assert the actual links
            }
        }

        [TestMethod]
        public void Can_Create_Owner()
        {
            var config = new TestConfig(); // todo - pull from container
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var testOwner = GetTestOwner();
                var expectedMetadata = new Metadata(new[]
                {
                    Link.Self($"{config.TokenizedBaseUrl}", HttpMethod.Post.Method),
                    Link.Custom("summary", $"{config.TokenizedBaseUrl}/{testOwner.Id}/summary", HttpMethod.Get.Method),
                    Link.Custom("detail", $"{config.TokenizedBaseUrl}/{testOwner.Id}/detail", HttpMethod.Get.Method),
                    Link.Edit($"{config.TokenizedBaseUrl}/{testOwner.Id}", HttpMethod.Put.Method),
                    Link.Delete($"{config.TokenizedBaseUrl}/{testOwner.Id}", HttpMethod.Delete.Method),
            });
                var expectedOwner = Mapper.Map<OwnerEntity, OwnerDetailModel>(testOwner);
                var expected = new Response(Result.Created, expectedMetadata);
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.Add(It.IsAny<OwnerEntity>())).ReturnsAsync(true);
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.Create(expectedOwner).Result;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Errors);
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.Created);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.AreEqual(expected.Metadata.Links.Count, actual.Metadata.Links.Count);
                // todo - compare and assert the actual links

            }
        }

        [TestMethod]
        public void Can_Update_Owner()
        {
            var config = new TestConfig(); // todo - pull from container
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var testOwner = GetTestOwner();
                var expectedMetadata = new Metadata(new[]
                {
                    Link.Self($"{config.TokenizedBaseUrl}", HttpMethod.Put.Method),
                    Link.Custom("summary", $"{config.TokenizedBaseUrl}/{testOwner.Id}/summary", HttpMethod.Get.Method),
                    Link.Custom("detail", $"{config.TokenizedBaseUrl}/{testOwner.Id}/detail", HttpMethod.Get.Method),
                    Link.Delete($"{config.TokenizedBaseUrl}/{testOwner.Id}", HttpMethod.Delete.Method),
                });
                var expectedOwner = Mapper.Map<OwnerEntity, OwnerDetailModel>(testOwner);
                var expected = new Response(Result.Updated, expectedMetadata);
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.Update(It.IsAny<OwnerEntity>())).ReturnsAsync(true);
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.Update(expectedOwner).Result;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Errors);
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.Updated);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.AreEqual(expected.Metadata.Links.Count, actual.Metadata.Links.Count);
                // todo - compare and assert the actual links
            }
        }


        [TestMethod]
        public void Can_DeleteOwner()
        {
            var config = new TestConfig(); // todo - pull from container
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var testOwner = GetTestOwner();
                var expectedMetadata = new Metadata(new[]
                {
                    Link.Self($"{config.TokenizedBaseUrl}", HttpMethod.Delete.Method),
                });
                var expected = new Response(Result.Deleted, expectedMetadata);
                mock.Mock<IRepository<OwnerEntity>>().Setup(x => x.Delete(testOwner.Id)).ReturnsAsync(true);
                var systemUnderTest = mock.Create<OwnerService>();

                // Act
                var actual = systemUnderTest.Delete(testOwner.Id).Result;

                // Assert
                mock.Mock<IRepository<OwnerEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Errors);
                Assert.AreEqual(expected.Result, actual.Result);
                Assert.AreEqual(actual.Result, Result.Deleted);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.AreEqual(expected.Metadata.Links.Count, actual.Metadata.Links.Count);
                // todo - compare and assert the actual links

            }
        }



        OwnerEntity GetTestOwner()
        {
            return new OwnerEntity
            {
                EmailAddress = "test@test.com",
                FirstName = "Test",
                LastName = "McTester"
            };
        }


    }


}
