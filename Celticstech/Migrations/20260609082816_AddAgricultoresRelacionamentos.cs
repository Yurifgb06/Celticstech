using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Celticstech.Migrations
{
    /// <inheritdoc />
    public partial class AddAgricultoresRelacionamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Agricultores");

            migrationBuilder.RenameColumn(
                name: "QtdeDependentes",
                table: "Agricultores",
                newName: "IdCultivo");

            migrationBuilder.RenameColumn(
                name: "Idade",
                table: "Agricultores",
                newName: "IdAssociacao");

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Agricultores",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Agricultores",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Agricultores_IdAssociacao",
                table: "Agricultores",
                column: "IdAssociacao");

            migrationBuilder.CreateIndex(
                name: "IX_Agricultores_IdCultivo",
                table: "Agricultores",
                column: "IdCultivo");

            migrationBuilder.AddForeignKey(
                name: "FK_Agricultores_Associacoes_IdAssociacao",
                table: "Agricultores",
                column: "IdAssociacao",
                principalTable: "Associacoes",
                principalColumn: "IdAssociacao",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agricultores_Cultivos_IdCultivo",
                table: "Agricultores",
                column: "IdCultivo",
                principalTable: "Cultivos",
                principalColumn: "IdCultivo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agricultores_Associacoes_IdAssociacao",
                table: "Agricultores");

            migrationBuilder.DropForeignKey(
                name: "FK_Agricultores_Cultivos_IdCultivo",
                table: "Agricultores");

            migrationBuilder.DropIndex(
                name: "IX_Agricultores_IdAssociacao",
                table: "Agricultores");

            migrationBuilder.DropIndex(
                name: "IX_Agricultores_IdCultivo",
                table: "Agricultores");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Agricultores");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Agricultores");

            migrationBuilder.RenameColumn(
                name: "IdCultivo",
                table: "Agricultores",
                newName: "QtdeDependentes");

            migrationBuilder.RenameColumn(
                name: "IdAssociacao",
                table: "Agricultores",
                newName: "Idade");

            migrationBuilder.AddColumn<string>(
                name: "Sexo",
                table: "Agricultores",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");
        }
    }
}
