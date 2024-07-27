using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addbranddynamicandSearchresult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "InterestRate",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "BrandDetailId",
                table: "Brands",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BrandDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    SliderImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderHref = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderImage1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderHref1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderImage2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderHref2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstBannerImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstBannerHref = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecBannerImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecBannerHref = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SearchResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Href = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchResults", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brands_BrandDetailId",
                table: "Brands",
                column: "BrandDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_BrandDetails_BrandDetailId",
                table: "Brands",
                column: "BrandDetailId",
                principalTable: "BrandDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_BrandDetails_BrandDetailId",
                table: "Brands");

            migrationBuilder.DropTable(
                name: "BrandDetails");

            migrationBuilder.DropTable(
                name: "SearchResults");

            migrationBuilder.DropIndex(
                name: "IX_Brands_BrandDetailId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "InterestRate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BrandDetailId",
                table: "Brands");
        }
    }
}
