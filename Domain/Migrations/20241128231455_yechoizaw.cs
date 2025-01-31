using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class yechoizaw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "FactorProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FactorProducts_BrandId",
                table: "FactorProducts",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_FactorProducts_Brands_BrandId",
                table: "FactorProducts",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactorProducts_Brands_BrandId",
                table: "FactorProducts");

            migrationBuilder.DropIndex(
                name: "IX_FactorProducts_BrandId",
                table: "FactorProducts");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "FactorProducts");
        }
    }
}
