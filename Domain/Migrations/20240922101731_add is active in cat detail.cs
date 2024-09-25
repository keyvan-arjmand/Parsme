using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addisactiveincatdetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandTags_BrandLandings_BrandLandingId",
                table: "BrandTags");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "BrandTags");

            migrationBuilder.AddColumn<string>(
                name: "TopBannerHref",
                table: "SeoPage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Features",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CategoryDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "BrandLandingId",
                table: "BrandTags",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandTags_BrandLandings_BrandLandingId",
                table: "BrandTags",
                column: "BrandLandingId",
                principalTable: "BrandLandings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandTags_BrandLandings_BrandLandingId",
                table: "BrandTags");

            migrationBuilder.DropColumn(
                name: "TopBannerHref",
                table: "SeoPage");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CategoryDetails");

            migrationBuilder.AlterColumn<int>(
                name: "BrandLandingId",
                table: "BrandTags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "BrandTags",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BrandTags_BrandLandings_BrandLandingId",
                table: "BrandTags",
                column: "BrandLandingId",
                principalTable: "BrandLandings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
