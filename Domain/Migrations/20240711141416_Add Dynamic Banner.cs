using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddDynamicBanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LargeSideBanner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LargeSideBannerHref = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmallSideBanner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmallSideBannerHref = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmallBannerMiddle1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmallBannerMiddle1Href = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmallBannerMiddle2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmallBannerMiddle2Href = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmallBannerMiddle3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmallBannerMiddle3Href = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmallBannerMiddle4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmallBannerMiddle4Href = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BigBannerMiddle1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BigBannerMiddle1Href = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BigBannerMiddle2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BigBannerMiddle2Href = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");
        }
    }
}
