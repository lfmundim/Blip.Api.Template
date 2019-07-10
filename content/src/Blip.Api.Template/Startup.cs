using Blip.Api.Template.Middleware;
using Lime.Protocol.Serialization.Newtonsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using SmallTalks.Core;
using SmallTalks.Core.Services;
using SmallTalks.Core.Services.Interfaces;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;
using Take.Blip.Client;
using Take.Blip.Client.Extensions.Broadcast;
using Take.Blip.Client.Extensions.Bucket;
using Take.Blip.Client.Extensions.Contacts;
using Take.Blip.Client.Extensions.Context;
using Take.Blip.Client.Extensions.Directory;
using Take.Blip.Client.Extensions.EventTracker;
using Take.Blip.Client.Extensions.Scheduler;
using Take.SmartContacts.Utils;

namespace Blip.Api.Template
{
    public class Startup
    {
        private const string SWAGGERFILE_PATH = "./swagger/v1/swagger.json";
        private const string API_VERSION = "v1";
        private const string SETTINGS_SECTION = "Settings";
        private const string APPLICATION_KEY = "Application";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Parsing appsettings into class
            var settings = Configuration.GetSection(SETTINGS_SECTION).Get<MySettings>();

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
                     .Enrich.WithProperty(APPLICATION_KEY, Models.Constants.PROJECT_NAME)
                     .Enrich.WithExceptionDetails()
                     .CreateLogger());

            // BLiP services registration
            var builder = new BlipClientBuilder();
            builder.UsingAccessKey(settings.BlipBotSettings.Identifier, settings.BlipBotSettings.AccessKey)
                .UsingRoutingRule(Lime.Messaging.Resources.RoutingRule.Instance)
                .WithChannelCount(2);

            var blipClient = builder.Build();
            services.AddSingleton<ISender>(blipClient);
            services.AddSingleton<ISchedulerExtension>(new SchedulerExtension(blipClient));
            services.AddSingleton<IBroadcastExtension>(new BroadcastExtension(blipClient));
            services.AddSingleton<IContextExtension>(new ContextExtension(blipClient));
            services.AddSingleton<IBucketExtension>(new BucketExtension(blipClient));
            services.AddSingleton<IDirectoryExtension>(new DirectoryExtension(blipClient));
            services.AddSingleton<IContactExtension>(new ContactExtension(blipClient));
            services.AddSingleton<IEventTrackExtension>(new EventTrackExtension(blipClient));


            // Project specific Services
            services.AddSingleton<IDataValidator, DataValidator>();
            services.AddSingleton<IDateTimeValidator, DateTimeValidator>();

            services.AddSingleton<ISmallTalksDetector, SmallTalksDetector>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IConversionService, ModelConversionService>();
            services.AddSingleton<IDetectorDataProviderService, DetectorDataProviderService>();
            services.AddSingleton<ISourceProviderService, LocalSourceProviderService>();
            services.AddSingleton<IWordDetectorFactory, WordDetectorFactory>();
            services.AddTransient<IWordsDetector, WordsDetectorBase>(); ;

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(API_VERSION, new Info { Title = Models.Constants.PROJECT_NAME, Version = API_VERSION });
                var xmlFile = Assembly.GetExecutingAssembly().GetName().Name + Models.Constants.XML_EXTENSION;
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

            // Use Error Handling Middleware to enable easy automated try-catch on Controller Actions
            app.UseMiddleware<ErrorHandlingMiddleware>();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint(SWAGGERFILE_PATH, Models.Constants.PROJECT_NAME + API_VERSION);
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
