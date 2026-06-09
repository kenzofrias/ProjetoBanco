using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoBanco.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicicionaTabelaContas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Conta",
                table: "Conta");

            migrationBuilder.RenameTable(
                name: "Conta",
                newName: "Contas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contas",
                table: "Contas",
                column: "Numero");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Contas",
                table: "Contas");

            migrationBuilder.RenameTable(
                name: "Contas",
                newName: "Conta");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Conta",
                table: "Conta",
                column: "Numero");
        }
    }
}
