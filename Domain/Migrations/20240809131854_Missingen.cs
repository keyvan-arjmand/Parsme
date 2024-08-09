using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class Missingen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_BrandDetails_BrandDetailId",
                table: "Brands");

            migrationBuilder.DropTable(
                name: "BrandDetails");

            migrationBuilder.DropIndex(
                name: "IX_Brands_BrandDetailId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "BrandDetailId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Desc",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "HrefCol23",
                table: "FooterLinks",
                newName: "HrefCol3");

            migrationBuilder.CreateTable(
                name: "BrandLandings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
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
                    SmallBanner1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmallBanner2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmallBanner3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmallBanner4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandLandings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandLandings_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandLandings_BrandId",
                table: "BrandLandings",
                column: "BrandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandLandings");

            migrationBuilder.RenameColumn(
                name: "HrefCol3",
                table: "FooterLinks",
                newName: "HrefCol23");

            migrationBuilder.AddColumn<int>(
                name: "BrandDetailId",
                table: "Brands",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BrandDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    FirstBannerHref = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstBannerImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    SecBannerHref = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecBannerImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderHref = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderHref1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderHref2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderImage1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderImage2 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandDetails", x => x.Id);
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
    }
}
