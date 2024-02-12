using Azure.Identity;
using Azure.Messaging.ServiceBus;
using ImageProcessor.Infrastructure.ConfigurationOptions;
using ImageProcessor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace ImageProcessor.Infrastructure.Extensions
{
    public static class InfrastructureServicesExtension
    {
        public static IServiceCollection AddContext(this IServiceCollection services, 
            string connectionString, 
            string migrationsAssembly = "")
        {
            services.AddDbContext<PostgresqlDbContext>(
                optionsBuilder =>
                    optionsBuilder.UseNpgsql(connectionString, opt => AddOptionbBuilderHandler(opt, migrationsAssembly))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
                ServiceLifetime.Transient);

            return services;
        }

        public static IServiceCollection AddServiceBusClient(this IServiceCollection services)
        {
            services.AddSingleton<ServiceBusClient>(p =>
            {
                var options = p.GetService<IOptions<QueueOptions>>();
                var clientOptions = new ServiceBusClientOptions
                {
                    TransportType = ServiceBusTransportType.AmqpWebSockets
                };
                return new ServiceBusClient(
                    connectionString: options.Value.ServiceBusConnection,
                    options: clientOptions);
            });

            return services;
        }

        private static void AddOptionbBuilderHandler(NpgsqlDbContextOptionsBuilder optionbBuilder, string migrationsAssembly = "")
        {
            optionbBuilder.EnableRetryOnFailure(
                   maxRetryCount: 3,
                   maxRetryDelay: TimeSpan.FromMilliseconds(100),
                   errorCodesToAdd: null);
            if (!string.IsNullOrEmpty(migrationsAssembly))
            {
                optionbBuilder.MigrationsAssembly(migrationsAssembly);
            }
        }
    }
}
