using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class LastChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Colors_ColorId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Offer_OfferId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offer",
                table: "Offer");

            migrationBuilder.RenameTable(
                name: "Offer",
                newName: "Offers");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_ColorId",
                table: "Offers",
                newName: "IX_Offers_ColorId");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Offers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offers",
                table: "Offers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Colors_ColorId",
                table: "Offers",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Offers_OfferId",
                table: "Products",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Colors_ColorId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Offers_OfferId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offers",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Offers");

            migrationBuilder.RenameTable(
                name: "Offers",
                newName: "Offer");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_ColorId",
                table: "Offer",
                newName: "IX_Offer_ColorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offer",
                table: "Offer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Colors_ColorId",
                table: "Offer",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Offer_OfferId",
                table: "Products",
                column: "OfferId",
                principalTable: "Offer",
                principalColumn: "Id");
        }
    }
}
