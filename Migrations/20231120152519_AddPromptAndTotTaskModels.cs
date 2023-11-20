using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OllamaToT.Migrations
{
    /// <inheritdoc />
    public partial class AddPromptAndTotTaskModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prompts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prompts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TotTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ProposePromptId = table.Column<int>(type: "integer", nullable: false),
                    EvaluatePromptId = table.Column<int>(type: "integer", nullable: false),
                    Response = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotTasks_Prompts_EvaluatePromptId",
                        column: x => x.EvaluatePromptId,
                        principalTable: "Prompts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TotTasks_Prompts_ProposePromptId",
                        column: x => x.ProposePromptId,
                        principalTable: "Prompts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TotTasks_EvaluatePromptId",
                table: "TotTasks",
                column: "EvaluatePromptId");

            migrationBuilder.CreateIndex(
                name: "IX_TotTasks_ProposePromptId",
                table: "TotTasks",
                column: "ProposePromptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TotTasks");

            migrationBuilder.DropTable(
                name: "Prompts");
        }
    }
}
