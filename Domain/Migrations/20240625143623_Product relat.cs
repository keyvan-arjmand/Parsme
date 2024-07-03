using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class Productrelat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_SubCategoryId",
                table: "Products",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ColorId",
                table: "ProductColors",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_GuaranteeId",
                table: "ProductColors",
                column: "GuaranteeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColors_Colors_ColorId",
                table: "ProductColors",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColors_Guarantees_GuaranteeId",
                table: "ProductColors",
                column: "GuaranteeId",
                principalTable: "Guarantees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductColors_Colors_ColorId",
                table: "ProductColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColors_Guarantees_GuaranteeId",
                table: "ProductColors");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SubCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductColors_ColorId",
                table: "ProductColors");

            migrationBuilder.DropIndex(
                name: "IX_ProductColors_GuaranteeId",
                table: "ProductColors");
        }
    }
}
