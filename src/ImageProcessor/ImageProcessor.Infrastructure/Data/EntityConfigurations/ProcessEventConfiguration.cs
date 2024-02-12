using ImageProcessor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace ImageProcessor.Infrastructure.Data.EntityConfigurations
{
    public class ProcessEventConfiguration : IEntityTypeConfiguration<ProcessEvent>
    {
        public void Configure(EntityTypeBuilder<ProcessEvent> builder)
        {
            builder.ToTable(
                name: "process_event",
                schema: "processor");

            builder.HasKey(x => x.EventId);

            builder.HasIndex(x => x.EventId)
               .IsUnique();

            builder.Property(x => x.EventId)
               .HasColumnName("event_id")
               .HasColumnType("uuid")
               .HasDefaultValueSql("uuid_generate_v4()")
               .IsRequired();

            builder
                .Property(x => x.FileId)
                .HasColumnName("file_id")
                .HasColumnType("uuid")
                .IsRequired();

            builder
                .Property(x => x.ProcessType)
                .HasColumnName("process_type")
                .HasColumnType("smallint")
                .IsRequired();

            builder
                .Property(x => x.ProcessStatus)
                .HasColumnName("process_status")
                .HasColumnType("smallint")
                .IsRequired();

            builder
                .Property(x => x.Input)
                .HasColumnName("input")
                .HasColumnType("json")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<object?>(v, new JsonSerializerOptions()));

            builder
                .Property(x => x.Output)
                .HasColumnName("output")
                .HasColumnType("json")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<object?>(v, new JsonSerializerOptions()));

            builder
                .Property(x => x.FaildMessage)
                .HasColumnName("faild_message")
                .HasColumnType("text");

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


        }
    }
}
