using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Celticstech.Migrations
{
    /// <inheritdoc />
    public partial class AjusteTipoRecomendacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TipoRecomendacao",
                table: "Recomendacoes",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TipoRecomendacao",
                table: "Recomendacoes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
