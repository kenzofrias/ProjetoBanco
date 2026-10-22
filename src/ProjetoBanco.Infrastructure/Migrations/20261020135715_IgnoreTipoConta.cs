using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoBanco.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IgnoreTipoConta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoConta",
                table: "Contas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoConta",
                table: "Contas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
