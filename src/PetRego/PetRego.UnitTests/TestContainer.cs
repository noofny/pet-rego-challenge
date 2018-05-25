using System;
using Autofac;
using PetRego.Data;
using PetRego.Common;

namespace PetRego.Api
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

            builder.RegisterType<ElasticClientFactory<OwnerEntity>>().As<IElasticClientFactory<OwnerEntity>>(); ;
            builder.RegisterType<ElasticClientFactory<PetEntity>>().As<IElasticClientFactory<PetEntity>>(); ;

            builder.RegisterType<ElasticClientFactory<OwnerEntity>>().As<IElasticClientFactory<OwnerEntity>>(); ;
            builder.RegisterType<ElasticClientFactory<PetEntity>>().As<IElasticClientFactory<PetEntity>>(); ;


            builder.RegisterType<OwnerService>();

            Container = builder.Build();

            _isConfigured = true;
        }

    }


}
