using ImageProcessor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageProcessor.Infrastructure.Data.EntityConfigurations
{
    public class FileMetadataConfiguration : IEntityTypeConfiguration<FileMetadata>
    {
        public void Configure(EntityTypeBuilder<FileMetadata> builder)
        {
            builder.ToTable(
                name: "file_metadata",
                schema: "processor");

            builder.HasKey(x => x.FileId);

            builder.HasIndex(x => x.FileId)
               .IsUnique();

            builder.Property(x => x.FileId)
               .HasColumnName("file_id")
               .HasColumnType("uuid")
               .HasDefaultValueSql("uuid_generate_v4()")
               .IsRequired();

            builder
                .Property(x => x.FileName)
                .HasColumnName("file_name")
                .HasColumnType("varchar(2048)")
                .IsRequired();

            builder
                .Property(x => x.FileType)
                .HasColumnName("file_type")
                .HasColumnType("smallint")
                .IsRequired();

            builder
                .Property(x => x.UpdatedDate)
                .HasColumnName("updated_date")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now() at time zone 'utc'")
                .IsRequired();

            builder
                .Property(x => x.CreatedDate)
                .HasColumnName("created_date")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now() at time zone 'utc'")
                .IsRequired();

            builder
                .HasMany(x => x.ProcessEvents)
                .WithOne(b => b.FileMetadata)
                .HasForeignKey(b => b.FileId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
