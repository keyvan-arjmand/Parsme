using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class ChnagesAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandLandings_Brands_BrandId",
                table: "BrandLandings");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                table: "BrandLandings",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BrandLandings_BrandId",
                table: "BrandLandings",
                newName: "IX_BrandLandings_CategoryId");

            migrationBuilder.AddColumn<string>(
                name: "SeoCanonical",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoDesc",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FirstCount",
                table: "DiscountCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SeoCanonical",
                table: "ContactUs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoDesc",
                table: "ContactUs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "ContactUs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoCanonical",
                table: "BrandLandings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoDesc",
                table: "BrandLandings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "BrandLandings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Adderss",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EconomicNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoCanonical",
                table: "AboutUsPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoDesc",
                table: "AboutUsPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "AboutUsPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandLandings_Categories_CategoryId",
                table: "BrandLandings",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandLandings_Categories_CategoryId",
                table: "BrandLandings");

            migrationBuilder.DropColumn(
                name: "SeoCanonical",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SeoDesc",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FirstCount",
                table: "DiscountCodes");

            migrationBuilder.DropColumn(
                name: "SeoCanonical",
                table: "ContactUs");

            migrationBuilder.DropColumn(
                name: "SeoDesc",
                table: "ContactUs");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "ContactUs");

            migrationBuilder.DropColumn(
                name: "SeoCanonical",
                table: "BrandLandings");

            migrationBuilder.DropColumn(
                name: "SeoDesc",
                table: "BrandLandings");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "BrandLandings");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Adderss",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EconomicNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrganizationName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrganizationNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SeoCanonical",
                table: "AboutUsPages");

            migrationBuilder.DropColumn(
                name: "SeoDesc",
                table: "AboutUsPages");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "AboutUsPages");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "BrandLandings",
                newName: "BrandId");

            migrationBuilder.RenameIndex(
                name: "IX_BrandLandings_CategoryId",
                table: "BrandLandings",
                newName: "IX_BrandLandings_BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandLandings_Brands_BrandId",
                table: "BrandLandings",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
