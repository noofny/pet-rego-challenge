using System;
using Autofac;
using PetRego.Api;
using PetRego.Data;
using PetRego.Common;

namespace PetRego.UnitTests
{
    public static class TestContainer
    {
        public static IContainer Container { get; private set; }
        static bool _isConfigured;

        public static void Configure()
        {
            if (_isConfigured)
            {
                return;
            }

            var builder = new ContainerBuilder();

            builder.RegisterInstance(new TestConfig()).As<IAppConfig>();
            builder.RegisterType<ElasticSearchRepository<OwnerEntity>>().As<IRepository<OwnerEntity>>();
            builder.RegisterType<ElasticSearchRepository<PetEntity>>().As<IRepository<PetEntity>>();
            builder.RegisterType<ElasticClientFactory<OwnerEntity>>();
            builder.RegisterType<ElasticClientFactory<PetEntity>>();
            builder.RegisterType<DiscoveryService>();
            builder.RegisterType<OwnerService>();
            builder.RegisterType<PetService>();
            builder.RegisterType<FoodService>();

            Container = builder.Build();

            _isConfigured = true;
        }

    }


}
