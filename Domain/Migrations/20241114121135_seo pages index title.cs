using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class seopagesindextitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactorProducts_Factors_FactorId",
                table: "FactorProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_FactorProducts_ProductColors_ProductColorId",
                table: "FactorProducts");

            migrationBuilder.DropIndex(
                name: "IX_FactorProducts_ProductColorId",
                table: "FactorProducts");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductColorId",
                table: "FactorProducts");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "FactorProducts",
                newName: "ProductId");

            migrationBuilder.AddColumn<string>(
                name: "ProductTitle",
                table: "SeoPage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductTitle2",
                table: "SeoPage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductTitle3",
                table: "SeoPage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductTitle4",
                table: "SeoPage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "FactorId",
                table: "FactorProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiscountAmount",
                table: "FactorProducts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUri",
                table: "FactorProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PersianTitle",
                table: "FactorProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "FactorProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UnicCode",
                table: "FactorProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FactorProductColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferAmount = table.Column<double>(type: "float", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Guarantee = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    FactorProductId = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactorProductColors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactorProductColors_FactorProducts_FactorProductId",
                        column: x => x.FactorProductId,
                        principalTable: "FactorProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FactorProductColors_FactorProductId",
                table: "FactorProductColors",
                column: "FactorProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_FactorProducts_Factors_FactorId",
                table: "FactorProducts",
                column: "FactorId",
                principalTable: "Factors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactorProducts_Factors_FactorId",
                table: "FactorProducts");

            migrationBuilder.DropTable(
                name: "FactorProductColors");

            migrationBuilder.DropColumn(
                name: "ProductTitle",
                table: "SeoPage");

            migrationBuilder.DropColumn(
                name: "ProductTitle2",
                table: "SeoPage");

            migrationBuilder.DropColumn(
                name: "ProductTitle3",
                table: "SeoPage");

            migrationBuilder.DropColumn(
                name: "ProductTitle4",
                table: "SeoPage");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "FactorProducts");

            migrationBuilder.DropColumn(
                name: "ImageUri",
                table: "FactorProducts");

            migrationBuilder.DropColumn(
                name: "PersianTitle",
                table: "FactorProducts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "FactorProducts");

            migrationBuilder.DropColumn(
                name: "UnicCode",
                table: "FactorProducts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "FactorProducts",
                newName: "Count");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "FactorId",
                table: "FactorProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProductColorId",
                table: "FactorProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FactorProducts_ProductColorId",
                table: "FactorProducts",
                column: "ProductColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_FactorProducts_Factors_FactorId",
                table: "FactorProducts",
                column: "FactorId",
                principalTable: "Factors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FactorProducts_ProductColors_ProductColorId",
                table: "FactorProducts",
                column: "ProductColorId",
                principalTable: "ProductColors",
                principalColumn: "Id");
        }
    }
}
