using ImageProcessor.Domain.Entities;
using ImageProcessor.Infrastructure.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace ImageProcessor.Infrastructure.Data.Context
{
    public class PostgresqlDbContext : DbContext
    {
        public DbSet<FileMetadata> FileMetadata { get; set; }
        public DbSet<ProcessEvent> ProcessEvents { get; set; }

        public PostgresqlDbContext(DbContextOptions<PostgresqlDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasPostgresExtension("uuid-ossp");
            builder.ApplyConfiguration(new FileMetadataConfiguration());
            builder.ApplyConfiguration(new ProcessEventConfiguration());
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => 
                    e.Entity is BaseEntity &&
                    e.State is EntityState.Added or EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.Entity is not BaseEntity entity) 
                {
                    continue;
                }

                entity.UpdatedDate = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedDate = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }
    }
}
