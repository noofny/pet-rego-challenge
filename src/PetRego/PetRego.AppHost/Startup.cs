using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using PetRego.Data;
using PetRego.Common;
using Swashbuckle.AspNetCore.Swagger;

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
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            applicationBuilder.UseMvc();
            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetRego REST API");
            });
            applicationLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                    Title = "PetRego REST API",
                    Description = "Unofficial REST API for the fictional PetRego business @ https://github.com/noofny/pet-rego-challenge",
                });
            });

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
            builder.RegisterType<DiscoveryService>();
            builder.RegisterType<OwnerService>();
            builder.RegisterType<PetService>();

            ApplicationContainer = builder.Build();


            AppHost.AutomapperConfig.Configure();

            return new AutofacServiceProvider(ApplicationContainer);
        }


    }

}
