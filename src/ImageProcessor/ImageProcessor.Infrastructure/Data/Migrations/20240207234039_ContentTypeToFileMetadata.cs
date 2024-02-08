using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageProcessor.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ContentTypeToFileMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "content_type",
                schema: "processor",
                table: "file_metadata",
                type: "varchar(2048)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "content_type",
                schema: "processor",
                table: "file_metadata");
        }
    }
}
