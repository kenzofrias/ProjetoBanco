using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoBanco.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueContaNumeroAgencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Contas_NumeroConta_Agencia",
                table: "Contas",
                columns: new[] { "NumeroConta", "Agencia" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contas_NumeroConta_Agencia",
                table: "Contas");
        }
    }
}