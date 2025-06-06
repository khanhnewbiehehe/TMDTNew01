using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThuongMaiDienTu.Migrations
{
    /// <inheritdoc />
    public partial class Update5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceItems_ProductTypes_ProductTypeId",
                table: "PriceItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductTypeId",
                table: "PriceItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceItems_ProductTypes_ProductTypeId",
                table: "PriceItems",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceItems_ProductTypes_ProductTypeId",
                table: "PriceItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductTypeId",
                table: "PriceItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceItems_ProductTypes_ProductTypeId",
                table: "PriceItems",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id");
        }
    }
}
