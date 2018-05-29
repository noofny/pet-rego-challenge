using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using PetRego.Data;
using PetRego.Common;

namespace PetRego.Api
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        public IConfigurationRoot Configuration { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = configurationBuilder.Build();
        }

        public void Configure(IApplicationBuilder applicationBuilder, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            applicationBuilder.UseMvc();
            applicationLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // all container register fun starts...
            var builder = new ContainerBuilder();
            builder.Populate(services);

            var appConfig = new AppConfig();
            Configuration.GetSection("AppConfig").Bind(appConfig);
            builder.RegisterInstance(appConfig).As<IAppConfig>();

            // todo - would be more graceful to register these based on some reflection magic...
            builder.RegisterType<ElasticSearchRepository<OwnerEntity>>().As<IRepository<OwnerEntity>>();
            builder.RegisterType<ElasticSearchRepository<PetEntity>>().As<IRepository<PetEntity>>();
            builder.RegisterType<ElasticClientFactory<OwnerEntity>>();
            builder.RegisterType<ElasticClientFactory<PetEntity>>();
            builder.RegisterType<OwnerService>();

            ApplicationContainer = builder.Build();



            AutomapperConfig.Configure();
            return new AutofacServiceProvider(ApplicationContainer);
        }


    }

}
