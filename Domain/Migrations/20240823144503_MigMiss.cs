using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class MigMiss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandLandings_Categories_CategoryId",
                table: "BrandLandings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BrandLandings",
                table: "BrandLandings");

            migrationBuilder.RenameTable(
                name: "BrandLandings",
                newName: "CatLandings");

            migrationBuilder.RenameIndex(
                name: "IX_BrandLandings_CategoryId",
                table: "CatLandings",
                newName: "IX_CatLandings_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatLandings",
                table: "CatLandings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CatLandings_Categories_CategoryId",
                table: "CatLandings",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatLandings_Categories_CategoryId",
                table: "CatLandings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatLandings",
                table: "CatLandings");

            migrationBuilder.RenameTable(
                name: "CatLandings",
                newName: "BrandLandings");

            migrationBuilder.RenameIndex(
                name: "IX_CatLandings_CategoryId",
                table: "BrandLandings",
                newName: "IX_BrandLandings_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BrandLandings",
                table: "BrandLandings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandLandings_Categories_CategoryId",
                table: "BrandLandings",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
