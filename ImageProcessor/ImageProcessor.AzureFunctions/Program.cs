using ImageProcessor.Application.Options;
using ImageProcessor.Application.Services;
using ImageProcessor.Domain.Entities;
using ImageProcessor.Domain.Interfaces.Repositories;
using ImageProcessor.Domain.Interfaces.Services;
using ImageProcessor.Infrastructure.ConfigurationOptions;
using ImageProcessor.Infrastructure.Data.Clients;
using ImageProcessor.Infrastructure.Data.Interfaces;
using ImageProcessor.Infrastructure.Data.Repositories;
using ImageProcessor.Infrastructure.Extensions;
using ImageProcessor.Infrastructure.Messaging.Interfaces;
using ImageProcessor.Infrastructure.Messaging.Producers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;

namespace ImageProcessor.AzureFunctions
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWebApplication()
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    var env = context.HostingEnvironment;
                    configBuilder
                        .AddJsonFile(Path.Combine(env.ContentRootPath, "local.settings.json"), optional: true, reloadOnChange: true)
                        .AddJsonFile(Path.Combine(env.ContentRootPath, $"local.settings.{env.EnvironmentName}.json"), optional: true, reloadOnChange: true);

                })
                .ConfigureServices((context, services) =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();

                    services
                        .AddOptions<BlobStorageOptions>()
                        .Bind(context.Configuration.GetSection("Values"));

                    services
                        .AddOptions<QueueOptions>()
                        .Bind(context.Configuration.GetSection("Values"));
                    services
                        .AddOptions<CVOptions>()
                        .Bind(context.Configuration.GetSection("Values"));

                    //DB
                    services.AddContext(context.Configuration);
                    services.AddScoped<IBlobStorageClient, BlobStorageClient>();
                    services.AddScoped<IRepository<FileMetadata, Guid>, FileMetadataRepository>();
                    services.AddScoped<IRepository<ProcessEvent, Guid>, ProcessEventRepository>();

                    //Messaging
                    services.AddSingleton<IMessageProducer, AzureMessageBusProducer>();

                    //Services
                    services.AddScoped<IVisionService, VisionService>();
                    services.AddScoped<IFileService, FileService>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}