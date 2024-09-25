using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addbrandtag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatLandings");

            migrationBuilder.AddColumn<int>(
                name: "BrandTagId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FaqCatId",
                table: "Faqs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FaqCats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqCats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrandLandings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandTagId = table.Column<int>(type: "int", nullable: true),
                    ImageSlider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSlider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSlider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescSlider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageSlider2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSlider2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSlider2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescSlider2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageSlider3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSlider3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSlider3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescSlider3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageSlider4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSlider4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSlider4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescSlider4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageSlider5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSlider5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSlider5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescSlider5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BigBanner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefBigBanner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmallBanner1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSmallBanner1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmallBanner2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSmallBanner2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmallBanner3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSmallBanner3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmallBanner4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSmallBanner4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeoTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandLandings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrandTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    BrandLandingId = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandTags_BrandLandings_BrandLandingId",
                        column: x => x.BrandLandingId,
                        principalTable: "BrandLandings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandTagId",
                table: "Products",
                column: "BrandTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Faqs_FaqCatId",
                table: "Faqs",
                column: "FaqCatId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandLandings_BrandTagId",
                table: "BrandLandings",
                column: "BrandTagId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandTags_BrandLandingId",
                table: "BrandTags",
                column: "BrandLandingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faqs_FaqCats_FaqCatId",
                table: "Faqs",
                column: "FaqCatId",
                principalTable: "FaqCats",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_BrandTags_BrandTagId",
                table: "Products",
                column: "BrandTagId",
                principalTable: "BrandTags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandLandings_BrandTags_BrandTagId",
                table: "BrandLandings",
                column: "BrandTagId",
                principalTable: "BrandTags",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faqs_FaqCats_FaqCatId",
                table: "Faqs");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_BrandTags_BrandTagId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_BrandLandings_BrandTags_BrandTagId",
                table: "BrandLandings");

            migrationBuilder.DropTable(
                name: "FaqCats");

            migrationBuilder.DropTable(
                name: "BrandTags");

            migrationBuilder.DropTable(
                name: "BrandLandings");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandTagId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Faqs_FaqCatId",
                table: "Faqs");

            migrationBuilder.DropColumn(
                name: "BrandTagId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FaqCatId",
                table: "Faqs");

            migrationBuilder.CreateTable(
                name: "CatLandings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    BigBanner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescSlider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescSlider2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescSlider3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescSlider4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescSlider5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefBigBanner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSlider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSlider2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSlider3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSlider4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSlider5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSmallBanner1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSmallBanner2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSmallBanner3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefSmallBanner4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageSlider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageSlider2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageSlider3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageSlider4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageSlider5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    SeoCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmallBanner1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmallBanner2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmallBanner3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmallBanner4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSlider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSlider2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSlider3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSlider4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSlider5 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatLandings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatLandings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatLandings_CategoryId",
                table: "CatLandings",
                column: "CategoryId");
        }
    }
}
