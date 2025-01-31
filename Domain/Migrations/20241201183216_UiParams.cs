using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class UiParams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ContactPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoCanonical",
                table: "ContactPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoDesc",
                table: "ContactPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "ContactPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsClick",
                table: "BrandLandings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "AboutUsPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ContactPages");

            migrationBuilder.DropColumn(
                name: "SeoCanonical",
                table: "ContactPages");

            migrationBuilder.DropColumn(
                name: "SeoDesc",
                table: "ContactPages");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "ContactPages");

            migrationBuilder.DropColumn(
                name: "IsClick",
                table: "BrandLandings");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "AboutUsPages");
        }
    }
}
