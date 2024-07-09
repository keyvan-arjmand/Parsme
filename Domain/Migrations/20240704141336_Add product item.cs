using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class Addproductitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Offer_ColorId",
                table: "Offer",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Colors_ColorId",
                table: "Offer",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Colors_ColorId",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_ColorId",
                table: "Offer");
        }
    }
}
