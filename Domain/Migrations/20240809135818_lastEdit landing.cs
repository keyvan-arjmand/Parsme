using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class lastEditlanding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HrefBigBanner",
                table: "BrandLandings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HrefSmallBanner1",
                table: "BrandLandings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HrefSmallBanner2",
                table: "BrandLandings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HrefSmallBanner3",
                table: "BrandLandings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HrefSmallBanner4",
                table: "BrandLandings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HrefBigBanner",
                table: "BrandLandings");

            migrationBuilder.DropColumn(
                name: "HrefSmallBanner1",
                table: "BrandLandings");

            migrationBuilder.DropColumn(
                name: "HrefSmallBanner2",
                table: "BrandLandings");

            migrationBuilder.DropColumn(
                name: "HrefSmallBanner3",
                table: "BrandLandings");

            migrationBuilder.DropColumn(
                name: "HrefSmallBanner4",
                table: "BrandLandings");
        }
    }
}
