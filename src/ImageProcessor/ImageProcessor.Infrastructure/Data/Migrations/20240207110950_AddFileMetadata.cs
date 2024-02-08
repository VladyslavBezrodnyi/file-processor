using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageProcessor.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFileMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                schema: "processor",
                table: "process_event",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_date",
                schema: "processor",
                table: "process_event",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.CreateTable(
                name: "file_metadata",
                schema: "processor",
                columns: table => new
                {
                    file_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    file_name = table.Column<string>(type: "varchar(2048)", nullable: false),
                    file_type = table.Column<short>(type: "smallint", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file_metadata", x => x.file_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_process_event_file_id",
                schema: "processor",
                table: "process_event",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "IX_file_metadata_file_id",
                schema: "processor",
                table: "file_metadata",
                column: "file_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_process_event_file_metadata_file_id",
                schema: "processor",
                table: "process_event",
                column: "file_id",
                principalSchema: "processor",
                principalTable: "file_metadata",
                principalColumn: "file_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_process_event_file_metadata_file_id",
                schema: "processor",
                table: "process_event");

            migrationBuilder.DropTable(
                name: "file_metadata",
                schema: "processor");

            migrationBuilder.DropIndex(
                name: "IX_process_event_file_id",
                schema: "processor",
                table: "process_event");

            migrationBuilder.DropColumn(
                name: "created_date",
                schema: "processor",
                table: "process_event");

            migrationBuilder.DropColumn(
                name: "updated_date",
                schema: "processor",
                table: "process_event");
        }
    }
}
