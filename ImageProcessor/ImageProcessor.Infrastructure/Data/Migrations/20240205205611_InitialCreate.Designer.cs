﻿// <auto-generated />
using System;
using ImageProcessor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ImageProcessor.Infrastructure.Data.Migrations
{
    [DbContext(typeof(PostgresqlDbContext))]
    [Migration("20240205205611_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ImageProcessor.Domain.Entities.ProcessEvent", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("event_id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("FaildMessage")
                        .HasColumnType("text")
                        .HasColumnName("faild_message");

                    b.Property<Guid>("FileId")
                        .HasColumnType("uuid")
                        .HasColumnName("file_id");

                    b.Property<string>("Input")
                        .HasColumnType("json")
                        .HasColumnName("input");

                    b.Property<string>("Output")
                        .HasColumnType("json")
                        .HasColumnName("output");

                    b.Property<short>("ProcessStatus")
                        .HasColumnType("smallint")
                        .HasColumnName("process_status");

                    b.Property<short>("ProcessType")
                        .HasColumnType("smallint")
                        .HasColumnName("process_type");

                    b.HasKey("EventId");

                    b.HasIndex("EventId")
                        .IsUnique();

                    b.ToTable("process_event", "processor");
                });
#pragma warning restore 612, 618
        }
    }
}
