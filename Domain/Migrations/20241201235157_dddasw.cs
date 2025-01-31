using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class dddasw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClick",
                table: "BrandLandings");

            migrationBuilder.AddColumn<bool>(
                name: "IsClick",
                table: "BrandTags",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClick",
                table: "BrandTags");

            migrationBuilder.AddColumn<bool>(
                name: "IsClick",
                table: "BrandLandings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
