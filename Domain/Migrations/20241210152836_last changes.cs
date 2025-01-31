using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class lastchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "ProductDetails");

            migrationBuilder.AddColumn<bool>(
                name: "IsPay",
                table: "Factors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPay",
                table: "Factors");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "ProductDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
