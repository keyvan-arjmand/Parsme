using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class DaynamicSlider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SliderHref",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SliderHref1",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SliderHref2",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SliderImage",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SliderImage1",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SliderImage2",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SliderTitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SliderTitle1",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SliderTitle2",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SliderHref",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "SliderHref1",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "SliderHref2",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "SliderImage",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "SliderImage1",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "SliderImage2",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "SliderTitle",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "SliderTitle1",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "SliderTitle2",
                table: "Banners");
        }
    }
}
