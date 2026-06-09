using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Celticstech.Migrations
{
    /// <inheritdoc />
    public partial class AddDadosClimaticosRecomendacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Chuva",
                table: "Recomendacoes",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FonteDados",
                table: "Recomendacoes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NivelRisco",
                table: "Recomendacoes",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Temperatura",
                table: "Recomendacoes",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Umidade",
                table: "Recomendacoes",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VelocidadeVento",
                table: "Recomendacoes",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Chuva",
                table: "Recomendacoes");

            migrationBuilder.DropColumn(
                name: "FonteDados",
                table: "Recomendacoes");

            migrationBuilder.DropColumn(
                name: "NivelRisco",
                table: "Recomendacoes");

            migrationBuilder.DropColumn(
                name: "Temperatura",
                table: "Recomendacoes");

            migrationBuilder.DropColumn(
                name: "Umidade",
                table: "Recomendacoes");

            migrationBuilder.DropColumn(
                name: "VelocidadeVento",
                table: "Recomendacoes");
        }
    }
}
