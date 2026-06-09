using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Celticstech.Migrations
{
    /// <inheritdoc />
    public partial class AddScoreRiscoRecomendacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScoreRisco",
                table: "Recomendacoes",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoreRisco",
                table: "Recomendacoes");
        }
    }
}
