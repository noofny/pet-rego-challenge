using System;
using Autofac;
using PetRego.Api;
using PetRego.Data;
using PetRego.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PetRego.UnitTests.ContainerTests
{
    [TestClass]
    public class ResolverTests
    {
        [TestInitialize]
        public void Setup()
        {
            TestContainer.Configure();
        }



        [TestMethod]
        public void Can_Resolve_IAppConfig()
        {
            var component = TestContainer.Container.Resolve<IAppConfig>();

            Assert.IsNotNull(component);
        }

        [TestMethod]
        public void Can_Resolve_IRepository_OwnerEntity()
        {
            var component = TestContainer.Container.Resolve<IRepository<OwnerEntity>>();

            Assert.IsNotNull(component);
        }

        [TestMethod]
        public void Can_Resolve_IRepository_PetEntity()
        {
            var component = TestContainer.Container.Resolve<IRepository<PetEntity>>();

            Assert.IsNotNull(component);
        }

        [TestMethod]
        public void Can_Resolve_ElasticClientFactory_OwnerEntity()
        {
            var component = TestContainer.Container.Resolve<ElasticClientFactory<OwnerEntity>>();

            Assert.IsNotNull(component);
        }

        [TestMethod]
        public void Can_Resolve_ElasticClientFactory_PetEntity()
        {
            var component = TestContainer.Container.Resolve<ElasticClientFactory<PetEntity>>();

            Assert.IsNotNull(component);
        }

        [TestMethod]
        public void Can_Resolve_DiscoveryService()
        {
            var component = TestContainer.Container.Resolve<DiscoveryService>();

            Assert.IsNotNull(component);
        }

        [TestMethod]
        public void Can_Resolve_OwnerService()
        {
            var component = TestContainer.Container.Resolve<OwnerService>();

            Assert.IsNotNull(component);
        }

        [TestMethod]
        public void Can_Resolve_PetService()
        {
            var component = TestContainer.Container.Resolve<PetService>();

            Assert.IsNotNull(component);
        }

        [TestMethod]
        public void Can_Resolve_FoodService()
        {
            var component = TestContainer.Container.Resolve<FoodService>();

            Assert.IsNotNull(component);
        }

    }
}
