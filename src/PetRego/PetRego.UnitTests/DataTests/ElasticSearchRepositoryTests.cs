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
using Autofac;
using System.Threading.Tasks;

namespace PetRego.UnitTests.DataTests
{
    [TestClass]
    public class ElasticSearchRepositoryTests
    {
        [TestInitialize]
        public void Setup()
        {
            AppHost.AutomapperConfig.Configure();
            TestContainer.Configure();
        }


        [TestMethod]
        [Ignore("Requires refactoring to work around complexities of Moq and async repo methods.")]
        public void Can_Search_Owners()
        {
            /* todo - refactoring is needed here to make the repo unit testable.
            *    
            * This causes the mocked client to respond with IsValid=false, with the following error;
            * "Invalid NEST response built from a null ApiCall which is highly exceptional, please open a bug if you see this\n"
            * This is because the DefaultIndex is an empty string. Changing this to a value then causes the 
            * mocked client to respond with a null response object. 
            * Have already spent a bit on this and very little help on the async support methods of Moq.
            * 
            * TL;DR - the lack of docs/examples for MOQ's async support is making this a bit of a trial. 
            */
            var clientMock = new Mock<Nest.IElasticClient>();
            var mockRequest = It.IsAny<Nest.Indices>();
            var mockRequestFunc = It.IsAny<Func<Nest.IndexExistsDescriptor, Nest.IndexExistsDescriptor>>();
            var mockResponse = Task.FromResult<Nest.IExistsResponse>(new Nest.ExistsResponse());
            clientMock.Setup(ec => ec.IndexExistsAsync(
                mockRequest, mockRequestFunc, new System.Threading.CancellationToken()))
                      .Returns(mockResponse);
            var systemUnderTest = new Mock<ElasticSearchRepository<OwnerEntity>>(clientMock.Object).Object;
            var actual = systemUnderTest.Search("id", Guid.NewGuid().ToString()).Result;
        }


    }
}
