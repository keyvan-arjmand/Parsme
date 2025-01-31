using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class remind : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRemind",
                table: "InventoryNotifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "InventoryNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryNotifications_ProductId",
                table: "InventoryNotifications",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryNotifications_Products_ProductId",
                table: "InventoryNotifications",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryNotifications_Products_ProductId",
                table: "InventoryNotifications");

            migrationBuilder.DropIndex(
                name: "IX_InventoryNotifications_ProductId",
                table: "InventoryNotifications");

            migrationBuilder.DropColumn(
                name: "IsRemind",
                table: "InventoryNotifications");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "InventoryNotifications");
        }
    }
}
