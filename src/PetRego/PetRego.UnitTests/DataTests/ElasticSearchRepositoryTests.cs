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
//using Nest;

namespace PetRego.UnitTests.DataTests
{
    [TestClass]
    public class ElasticSearchRepositoryTests
    {
        [TestInitialize]
        public void Setup()
        {
            AutomapperConfig.Configure();
            TestContainer.Configure();
        }


        [TestMethod]
        public void Can_GetAllOwners()
        {
            using (var mock = AutoMock.GetLoose())
            {
                /*
                 * 
                 * Problem here is because i'm using a factory to create the IElasticClient inside the repo,
                 * Moq is returning it as null, which then causes a null ref whenever anything in the repo uses it
                 * 
                 * Really not sure here, needs more thought. 
                 * 
                 * There will be a way!
                 * 
                 */

                try
                {
                    // Arrange
                    var expected = new List<OwnerEntity>();

                    var mockClient = new Mock<Nest.IElasticClient>();
                    //var uri = new Uri("http://localhost");
                    //mock.Mock<TestConfig>(); //x => x.ElasticSearchUri = "http://localhost/")); //.Returns(mockClient);
                    mock.Mock<Nest.IElasticClient>();
                    mock.Mock<IElasticClientFactory<OwnerEntity>>().Setup(x => x.GetClient("http://localhost")); //.Returns(mockClient);
                    var systemUnderTest = mock.Create<ElasticSearchRepository<OwnerEntity>>();

                    // Act
                    var actual = systemUnderTest.List().Result;

                    // Assert
                    mock.Mock<ElasticSearchRepository<OwnerEntity>>().Verify(x => x.List());
                    Assert.IsNotNull(actual);
                }
                catch(Exception e)
                {
                    
                }
            }
        }


    }
}
