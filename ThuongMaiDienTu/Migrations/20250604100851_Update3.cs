using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThuongMaiDienTu.Migrations
{
    /// <inheritdoc />
    public partial class Update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProduct_Favorites_FavouriteId",
                table: "FavouriteProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProduct_Products_ProductId",
                table: "FavouriteProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavouriteProduct",
                table: "FavouriteProduct");

            migrationBuilder.RenameTable(
                name: "FavouriteProduct",
                newName: "FavouriteProducts");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteProduct_ProductId",
                table: "FavouriteProducts",
                newName: "IX_FavouriteProducts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavouriteProducts",
                table: "FavouriteProducts",
                columns: new[] { "FavouriteId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProducts_Favorites_FavouriteId",
                table: "FavouriteProducts",
                column: "FavouriteId",
                principalTable: "Favorites",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProducts_Products_ProductId",
                table: "FavouriteProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProducts_Favorites_FavouriteId",
                table: "FavouriteProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProducts_Products_ProductId",
                table: "FavouriteProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavouriteProducts",
                table: "FavouriteProducts");

            migrationBuilder.RenameTable(
                name: "FavouriteProducts",
                newName: "FavouriteProduct");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteProducts_ProductId",
                table: "FavouriteProduct",
                newName: "IX_FavouriteProduct_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavouriteProduct",
                table: "FavouriteProduct",
                columns: new[] { "FavouriteId", "ProductId" });

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
    }
}
