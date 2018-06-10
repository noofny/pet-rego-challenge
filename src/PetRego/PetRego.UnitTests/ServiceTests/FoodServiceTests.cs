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
    public class FoodServiceTests : ServiceTestsBase
    {
        
        [TestMethod]
        public void Can_Get_Summary_With_Grouping()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var config = mock.Provide<IAppConfig, TestConfig>();
                var groupedBy = "Type";
                var testPet = GetTestPet();
                var expectedMetadata = new Metadata(new[]
                {
                    Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method),
                });
                var expectedCount = new EntityCount(groupedBy, new Dictionary<string, long>
                {
                    { PetType.Cat.ToString(), 1 }
                });
                var expectedFoodSummaryModel = new FoodSummaryModel();
                expectedFoodSummaryModel.FoodCounts.Add(new FoodCount(FoodType.Fish, 1));
                var expected = new SingleResponse(Result.Success, expectedMetadata, expectedFoodSummaryModel);
                mock.Mock<IRepository<PetEntity>>().Setup(x => x.Count<PetType>(groupedBy)).ReturnsAsync(expectedCount);
                var systemUnderTest = mock.Create<FoodService>();

                // Act
                var actual = systemUnderTest.Summary().Result;

                // Assert
                mock.Mock<IRepository<PetEntity>>().VerifyAll();
                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Errors);
                Assert.IsNotNull(actual.Data);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.AreEqual(actual.Result, Result.Success);
                var actualFoodSummary = actual.Data as FoodSummaryModel;
                Assert.IsNotNull(actualFoodSummary.FoodCounts);
                Assert.AreEqual(expectedFoodSummaryModel.FoodCounts.Count, actualFoodSummary.FoodCounts.Count);
                Assert.IsTrue(FoodCountsAreEqual(expectedFoodSummaryModel.FoodCounts, actualFoodSummary.FoodCounts));
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.IsTrue(MetadataLinksAreEqual(expected.Metadata.Links, actual.Metadata.Links));
            }
        }


        bool FoodCountsAreEqual(List<FoodCount> expected, List<FoodCount> actual)
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
            foreach (var expectedItem in expected)
            {
                var actualItem = actual.SingleOrDefault(x =>
                                   x.Type == expectedItem.Type &&
                                   x.Count == expectedItem.Count 
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
