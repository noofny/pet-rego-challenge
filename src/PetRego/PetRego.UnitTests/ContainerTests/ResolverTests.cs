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
        public void Can_Resolve_IElasticClientFactory_OwnerEntity()
        {
            var component = TestContainer.Container.Resolve<IElasticClientFactory<OwnerEntity>>();

            Assert.IsNotNull(component);
        }

        [TestMethod]
        public void Can_Resolve_IElasticClientFactory_PetEntity()
        {
            var component = TestContainer.Container.Resolve<IElasticClientFactory<PetEntity>>();

            Assert.IsNotNull(component);
        }

        [TestMethod]
        public void Can_Resolve_OwnerService()
        {
            var component = TestContainer.Container.Resolve<OwnerService>();

            Assert.IsNotNull(component);
        }


    }
}
