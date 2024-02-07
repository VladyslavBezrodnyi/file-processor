using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageProcessor.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "processor");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "process_event",
                schema: "processor",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    process_type = table.Column<short>(type: "smallint", nullable: false),
                    process_status = table.Column<short>(type: "smallint", nullable: false),
                    input = table.Column<string>(type: "json", nullable: true),
                    output = table.Column<string>(type: "json", nullable: true),
                    faild_message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_process_event", x => x.event_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_process_event_event_id",
                schema: "processor",
                table: "process_event",
                column: "event_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "process_event",
                schema: "processor");
        }
    }
}
