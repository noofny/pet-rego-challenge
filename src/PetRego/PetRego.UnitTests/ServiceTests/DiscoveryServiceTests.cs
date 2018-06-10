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
    public class DiscoveryServiceTests : ServiceTestsBase
    {
        
        [TestMethod]
        public void Can_Get_Discovery()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var config = mock.Provide<IAppConfig, TestConfig>();
                var expectedMetadata = new Metadata(new[]
                {
                    Link.Self($"{Constants.TOKENIZED_CURRENT_URL}", HttpMethod.Get.Method),

                    Link.Custom("owners-search", $"{Constants.TOKENIZED_CURRENT_URL}/owner/search/{{field}}/{{value}}", HttpMethod.Get.Method),
                    Link.Custom("owners-by-email", $"{Constants.TOKENIZED_CURRENT_URL}/owner/search/emailAddress/{{value}}", HttpMethod.Get.Method),
                    Link.Custom("owners-by-first-name", $"{Constants.TOKENIZED_CURRENT_URL}/owner/search/firstName/{{value}}", HttpMethod.Get.Method),
                    Link.Custom("owners-by-last-name", $"{Constants.TOKENIZED_CURRENT_URL}/owner/search/lastName/{{value}}", HttpMethod.Get.Method),
                    Link.Custom("owners-by-phone", $"{Constants.TOKENIZED_CURRENT_URL}/owner/search/phoneNumber/{{value}}", HttpMethod.Get.Method),

                    Link.Custom("pets-search", $"{Constants.TOKENIZED_CURRENT_URL}/pet/search/{{field}}/{{value}}", HttpMethod.Get.Method),
                    Link.Custom("pets-by-name", $"{Constants.TOKENIZED_CURRENT_URL}/pet/search/name/{{value}}", HttpMethod.Get.Method),
                    Link.Custom("pets-by-type", $"{Constants.TOKENIZED_CURRENT_URL}/pet/search/type/{{value}}", HttpMethod.Get.Method),
                    Link.Custom("pets-by-breed", $"{Constants.TOKENIZED_CURRENT_URL}/pet/search/breed/{{value}}", HttpMethod.Get.Method),

                    Link.Custom("food-summary", $"{Constants.TOKENIZED_CURRENT_URL}/food/summary", HttpMethod.Get.Method),
                });
                var expected = new Response(Result.Success, expectedMetadata);
                var systemUnderTest = mock.Create<DiscoveryService>();

                // Act
                var actual = systemUnderTest.Discover();

                // Assert
                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Errors);
                Assert.IsNotNull(actual.Metadata);
                Assert.IsNotNull(actual.Metadata.Links);
                Assert.AreEqual(actual.Result, Result.Success);
                Assert.AreEqual(expected.Metadata.ServerVersion, actual.Metadata.ServerVersion);
                Assert.IsTrue(MetadataLinksAreEqual(expected.Metadata.Links, actual.Metadata.Links));
            }
        }

    }


}
