using ImageProcessor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace ImageProcessor.Infrastructure.Extensions
{
    public static class InfrastructureServicesExtension
    {
        public static IServiceCollection AddContext(this IServiceCollection services, 
            IConfiguration configuration, 
            string migrationsAssembly = "")
        {
            var connectionString = configuration.GetConnectionString("PostgresqlConnectionString");
            services.AddDbContext<PostgresqlDbContext>(
                optionsBuilder =>
                    optionsBuilder.UseNpgsql(connectionString, opt => AddOptionbBuilderHandler(opt, migrationsAssembly))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
                ServiceLifetime.Transient);

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
