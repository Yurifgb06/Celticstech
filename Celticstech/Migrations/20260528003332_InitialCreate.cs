using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Celticstech.Migrations
{
  
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agricultores",
                columns: table => new
                {
                    IdAgricultor = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeAgricultor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Idade = table.Column<int>(type: "integer", nullable: false),
                    Sexo = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    QtdeDependentes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agricultores", x => x.IdAgricultor);
                });

            migrationBuilder.CreateTable(
                name: "Cultivos",
                columns: table => new
                {
                    IdCultivo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeCultivo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CategoriaCultivo = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    PorteCultivo = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    TempoColheita = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    VidaUtil = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Intermitencia = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cultivos", x => x.IdCultivo);
                });

            migrationBuilder.CreateTable(
                name: "Regioes",
                columns: table => new
                {
                    IdRegiao = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeRegiao = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    UfRegiao = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regioes", x => x.IdRegiao);
                });

            migrationBuilder.CreateTable(
                name: "Associacoes",
                columns: table => new
                {
                    IdAssociacao = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeAssociacao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SiglaAssociacao = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    IdRegiao = table.Column<int>(type: "integer", nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Login = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Senha = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Associacoes", x => x.IdAssociacao);
                    table.ForeignKey(
                        name: "FK_Associacoes_Regioes_IdRegiao",
                        column: x => x.IdRegiao,
                        principalTable: "Regioes",
                        principalColumn: "IdRegiao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recomendacoes",
                columns: table => new
                {
                    IdRecomendacao = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataRecAsc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdAssociacao = table.Column<int>(type: "integer", nullable: false),
                    IdCultivo = table.Column<int>(type: "integer", nullable: false),
                    Orientacao = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TipoRecomendacao = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recomendacoes", x => x.IdRecomendacao);
                    table.ForeignKey(
                        name: "FK_Recomendacoes_Associacoes_IdAssociacao",
                        column: x => x.IdAssociacao,
                        principalTable: "Associacoes",
                        principalColumn: "IdAssociacao",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recomendacoes_Cultivos_IdCultivo",
                        column: x => x.IdCultivo,
                        principalTable: "Cultivos",
                        principalColumn: "IdCultivo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Associacoes_IdRegiao",
                table: "Associacoes",
                column: "IdRegiao");

            migrationBuilder.CreateIndex(
                name: "IX_Recomendacoes_IdAssociacao",
                table: "Recomendacoes",
                column: "IdAssociacao");

            migrationBuilder.CreateIndex(
                name: "IX_Recomendacoes_IdCultivo",
                table: "Recomendacoes",
                column: "IdCultivo");
        }

       
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agricultores");

            migrationBuilder.DropTable(
                name: "Recomendacoes");

            migrationBuilder.DropTable(
                name: "Associacoes");

            migrationBuilder.DropTable(
                name: "Cultivos");

            migrationBuilder.DropTable(
                name: "Regioes");
        }
    }
}
