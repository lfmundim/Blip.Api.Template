using Blip.Api.Template.Models;
using Blip.Api.Template.Services;
using Blip.HttpClient.Extensions;
using Lime.Protocol;
using Lime.Protocol.Serialization.Newtonsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Blip.Api.Template
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Parsing appsettings into class
            var settings = Configuration.GetSection("Settings").Get<MySettings>();

            // Adds BLiP's Json Serializer to use on BLiP's Builder
            services.AddMvc().AddJsonOptions(options =>
            {
                foreach (var settingsConverter in JsonNetSerializer.Settings.Converters)
                {
                    options.SerializerSettings.Converters.Add(settingsConverter);
                }
            });

            // Dependency injection
            services.AddSingleton(settings);
            services.AddSingleton(settings.BlipBotSettings);

            // SERILOG settings
            services.AddSingleton<ILogger>(new LoggerConfiguration()
                     .ReadFrom.Configuration(Configuration)
                     .Enrich.WithMachineName()
                     .Enrich.WithProperty("Application", "Blip.Api.Template")
                     .Enrich.WithExceptionDetails()
                     .CreateLogger());

            // BLiP services registration
            var documentList = new List<Document>();
            documentList.Add(new UserContext());
            services.DefaultRegister(settings.BlipBotSettings.Authorization, documentList);

            // Project specific Services
            services.AddSingleton<IContextManager, ContextManager>();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Blip.Api.Template", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("./swagger/v1/swagger.json", "Blip.Api.Template V1");
            });

            app.UseMvc();
        }
    }
}
