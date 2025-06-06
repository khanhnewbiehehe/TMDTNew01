using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThuongMaiDienTu.Migrations
{
    /// <inheritdoc />
    public partial class Update_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Deposit",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ToTal",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deposit",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ToTal",
                table: "Invoices");
        }
    }
}
