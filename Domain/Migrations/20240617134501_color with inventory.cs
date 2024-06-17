using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class colorwithinventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inventory",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageUri",
                table: "ProductColors");

            migrationBuilder.AddColumn<int>(
                name: "Inventory",
                table: "ProductColors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inventory",
                table: "ProductColors");

            migrationBuilder.AddColumn<int>(
                name: "Inventory",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUri",
                table: "ProductColors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
