using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.ShoppingCartAPI.Migrations
{
    public partial class AddProductsToShopppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_Product_ProductId",
                table: "CartDetails");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_Products_ProductId",
                table: "CartDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_Products_ProductId",
                table: "CartDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "ProductId");

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CardHeaderCartHeaderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_Carts_CartHeader_CardHeaderCartHeaderId",
                        column: x => x.CardHeaderCartHeaderId,
                        principalTable: "CartHeader",
                        principalColumn: "CartHeaderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CardHeaderCartHeaderId",
                table: "Carts",
                column: "CardHeaderCartHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_Product_ProductId",
                table: "CartDetails",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
