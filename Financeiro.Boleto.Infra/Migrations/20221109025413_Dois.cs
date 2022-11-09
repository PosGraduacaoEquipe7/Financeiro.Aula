using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financeiro.Boleto.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Dois : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumeroBoleto",
                table: "Boletos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroBoleto",
                table: "Boletos");
        }
    }
}
