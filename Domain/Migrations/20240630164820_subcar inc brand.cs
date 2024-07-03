using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class subcarincbrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "Brands",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_SubCategoryId",
                table: "Brands",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_SubCategories_SubCategoryId",
                table: "Brands",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_SubCategories_SubCategoryId",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Brands_SubCategoryId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "Brands");
        }
    }
}
