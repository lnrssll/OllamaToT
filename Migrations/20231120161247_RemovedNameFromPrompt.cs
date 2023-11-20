using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OllamaToT.Migrations
{
    /// <inheritdoc />
    public partial class RemovedNameFromPrompt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Prompts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Prompts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
