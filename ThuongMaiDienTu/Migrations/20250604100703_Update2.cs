using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThuongMaiDienTu.Migrations
{
    /// <inheritdoc />
    public partial class Update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProduct_Favorites_FavouritesUserId",
                table: "FavouriteProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProduct_Products_ProductsId",
                table: "FavouriteProduct");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "FavouriteProduct",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "FavouritesUserId",
                table: "FavouriteProduct",
                newName: "FavouriteId");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteProduct_ProductsId",
                table: "FavouriteProduct",
                newName: "IX_FavouriteProduct_ProductId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "FavouriteProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProduct_Favorites_FavouriteId",
                table: "FavouriteProduct",
                column: "FavouriteId",
                principalTable: "Favorites",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProduct_Products_ProductId",
                table: "FavouriteProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProduct_Favorites_FavouriteId",
                table: "FavouriteProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProduct_Products_ProductId",
                table: "FavouriteProduct");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "FavouriteProduct");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "FavouriteProduct",
                newName: "ProductsId");

            migrationBuilder.RenameColumn(
                name: "FavouriteId",
                table: "FavouriteProduct",
                newName: "FavouritesUserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteProduct_ProductId",
                table: "FavouriteProduct",
                newName: "IX_FavouriteProduct_ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProduct_Favorites_FavouritesUserId",
                table: "FavouriteProduct",
                column: "FavouritesUserId",
                principalTable: "Favorites",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProduct_Products_ProductsId",
                table: "FavouriteProduct",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
