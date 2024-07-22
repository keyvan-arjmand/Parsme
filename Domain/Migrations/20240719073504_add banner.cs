using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addbanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BigBannerMiddle1Col",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BigBannerMiddle1ColHref",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BigBannerMiddle2Col",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BigBannerMiddle2ColHref",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BigBannerMiddle1Col",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "BigBannerMiddle1ColHref",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "BigBannerMiddle2Col",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "BigBannerMiddle2ColHref",
                table: "Banners");
        }
    }
}
