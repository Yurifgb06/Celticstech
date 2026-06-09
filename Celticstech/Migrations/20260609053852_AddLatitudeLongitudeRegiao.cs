using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Celticstech.Migrations
{
    /// <inheritdoc />
    public partial class AddLatitudeLongitudeRegiao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Regioes",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Regioes",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Regioes");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Regioes");
        }
    }
}
