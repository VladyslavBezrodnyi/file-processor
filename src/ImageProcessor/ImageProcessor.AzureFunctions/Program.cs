using Azure.Core.Serialization;
using ImageProcessor.Application.Extensions;
using ImageProcessor.Application.Options;
using ImageProcessor.Application.Services;
using ImageProcessor.Application.Services.Interfaces;
using ImageProcessor.Domain.Entities;
using ImageProcessor.Infrastructure.ConfigurationOptions;
using ImageProcessor.Infrastructure.Data.Clients;
using ImageProcessor.Infrastructure.Data.Interfaces;
using ImageProcessor.Infrastructure.Data.Repositories;
using ImageProcessor.Infrastructure.Extensions;
using ImageProcessor.Infrastructure.Messaging.Interfaces;
using ImageProcessor.Infrastructure.Messaging.Producers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ImageProcessor.AzureFunctions
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWebApplication(ConfigureWorkerDelegate)
                //.ConfigureFunctionsWorkerDefaults(ConfigureWorkerDelegate, WorkerOptionsDelegate)
                .ConfigureAppConfiguration(ConfigureAppConfigurationDelegate)
                .ConfigureServices(ConfigureServicesDelegate)
                .Build();

            await host.RunAsync();
        }

        private static void ConfigureWorkerDelegate(HostBuilderContext context, IFunctionsWorkerApplicationBuilder builder)
        {
        }

        private static void WorkerOptionsDelegate(WorkerOptions configureOptions)
        {
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new JsonStringEnumConverter());
            configureOptions.Serializer = new JsonObjectSerializer(serializeOptions);
        }

        private static void ConfigureAppConfigurationDelegate(HostBuilderContext context, IConfigurationBuilder configBuilder)
        {
            var env = context.HostingEnvironment;
            configBuilder
                .AddJsonFile(Path.Combine(env.ContentRootPath, "local.settings.json"), optional: true, reloadOnChange: true)
                .AddJsonFile(Path.Combine(env.ContentRootPath, $"local.settings.{env.EnvironmentName}.json"), optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        private static void ConfigureServicesDelegate(HostBuilderContext context, IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetryWorkerService();
            services.ConfigureFunctionsApplicationInsights();

            ConfigureOpyions(services);

            services.AddMapping();
            services.AddCVClient();
            services.AddServiceBusClient();
            services.AddContext(context.Configuration["PostgresqlConnectionString"] ?? throw new ArgumentNullException());

            services.AddSignalR().AddAzureSignalR();

            //DB
            services.AddScoped<IBlobStorageClient, BlobStorageClient>();
            services.AddScoped<IRepository<FileMetadata, Guid>, FileMetadataRepository>();
            services.AddScoped<IRepository<ProcessEvent, Guid>, ProcessEventRepository>();

            //Messaging
            services.AddSingleton<IMessageProducer, AzureMessageBusProducer>();

            //Services
            services.AddScoped<IVisionService, VisionService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IProcessEventService, ProcessEventService>();
        }

        private static void ConfigureOpyions(IServiceCollection services)
        {
            services
                .AddOptions<BlobStorageOptions>()
                .Configure<IConfiguration>((options, config) => config.Bind(options));
            services
                .AddOptions<QueueOptions>()
                .Configure<IConfiguration>((options, config) => config.Bind(options));
            services
                .AddOptions<CVOptions>()
                .Configure<IConfiguration>((options, config) => config.Bind(options));
        }
    }
}