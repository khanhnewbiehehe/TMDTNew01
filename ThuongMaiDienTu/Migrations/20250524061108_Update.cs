using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThuongMaiDienTu.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductLaunchs_Products_ProductId",
                table: "ProductLaunchs");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductLaunchs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLaunchs_Products_ProductId",
                table: "ProductLaunchs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductLaunchs_Products_ProductId",
                table: "ProductLaunchs");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductLaunchs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLaunchs_Products_ProductId",
                table: "ProductLaunchs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
