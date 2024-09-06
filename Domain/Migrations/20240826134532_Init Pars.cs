using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class InitPars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUsPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Head = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

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
                    BigBannerMiddle1Col = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BigBannerMiddle1ColHref = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BigBannerMiddle2Col = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BigBannerMiddle2ColHref = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderHref = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderTitle1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderImage1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderHref1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderTitle2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderImage2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderHref2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SeoTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscountCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    FirstCount = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Faqs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faqs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FooterLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescCol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleCol1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefCol1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title1Col1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href1Col1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title2Col1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href2Col1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title3Col1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href3Col1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescCol2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleCol2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefCol2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title1Col2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href1Col2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title2Col2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href2Col2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title3Col2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href3Col2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescCol3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleCol3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefCol3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title1Col3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href1Col3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title2Col3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href2Col3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title3Col3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href3Col3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescFooter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FooterPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhyParsDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoWhyParsTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoWhyParsDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoWhyParsCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParsDarYekDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsDarYekTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsDarYekDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsDarYekCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParsGoalsDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsGoalsTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsGoalsDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsGoalsCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParsInstallmentPurchaseDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsInstallmentPurchaseTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsInstallmentPurchaseDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsInstallmentPurchaseCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParsBuyingGuideDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsBuyingGuideTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsBuyingGuideDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsBuyingGuideCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParsOrganizationalPurchaseDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsOrganizationalPurchaseTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsOrganizationalPurchaseDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsOrganizationalPurchaseCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParsWarrantyDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsWarrantyTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsWarrantyDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsWarrantyCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParsShippingMethodsDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsShippingMethodsTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsShippingMethodsDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsShippingMethodsCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParsConsultationBeforePurchaseDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsConsultationBeforePurchaseTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsConsultationBeforePurchaseDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsConsultationBeforePurchaseCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParsProceduresForReturningGoodsDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsProceduresForReturningGoodsTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsProceduresForReturningGoodsDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsProceduresForReturningGoodsCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParsTrackingOrdersDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsTrackingOrdersGoodsTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsTrackingOrdersGoodsDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsTrackingOrdersGoodsCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParsOnlineSupportDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsOnlineSupportTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsOnlineSupportDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsOnlineSupportCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParsRegulationsDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsRegulationsTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsRegulationsDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsRegulationsCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParsUserPrivacyDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsUserPrivacyTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsUserPrivacyDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoParsUserPrivacyCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Guarantees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guarantees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductColorId = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Desc1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc1Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc2Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc3Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc4Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc5Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleServices", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "SeoPage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeoIndexTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoIndexDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeoIndexCanonical = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeoPage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatLandings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_CatLandings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatLandings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Days = table.Column<int>(type: "int", nullable: false),
                    Hours = table.Column<int>(type: "int", nullable: false),
                    Minutes = table.Column<int>(type: "int", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    OfferAmount = table.Column<double>(type: "float", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Brands_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CategoryDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataType = table.Column<int>(type: "int", nullable: false),
                    Option = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowInSearch = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryDetails_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryDetails_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    ConfirmCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sheba = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmCodeExpireTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    EconomicNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adderss = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersianTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetaKeyword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductGift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Strengths = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeakPoints = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnicCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    InterestRate = table.Column<double>(type: "float", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: true),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductStatus = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsOffer = table.Column<bool>(type: "bit", nullable: false),
                    IsShowIndex = table.Column<bool>(type: "bit", nullable: false),
                    MomentaryOffer = table.Column<bool>(type: "bit", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SeoTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeoDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeoCanonical = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCategoryDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false),
                    CategoryDetailId = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategoryDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategoryDetails_CategoryDetails_CategoryDetailId",
                        column: x => x.CategoryDetailId,
                        principalTable: "CategoryDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubCategoryDetails_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAddressId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    PostMethodId = table.Column<int>(type: "int", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FactorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    AmountPrice = table.Column<double>(type: "float", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsReturned = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factors_Addresses_UserAddressId",
                        column: x => x.UserAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Factors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Factors_PostMethods_PostMethodId",
                        column: x => x.PostMethodId,
                        principalTable: "PostMethods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImageGalleries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageGalleries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageGalleries_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    GuaranteeId = table.Column<int>(type: "int", nullable: false),
                    Inventory = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductColors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductColors_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductColors_Guarantees_GuaranteeId",
                        column: x => x.GuaranteeId,
                        principalTable: "Guarantees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductColors_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryDetailId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDetails_CategoryDetails_CategoryDetailId",
                        column: x => x.CategoryDetailId,
                        principalTable: "CategoryDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFavs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFavs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogFactors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FactorId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogFactors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogFactors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogFactors_Factors_FactorId",
                        column: x => x.FactorId,
                        principalTable: "Factors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnedFactors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnedType = table.Column<int>(type: "int", nullable: false),
                    ReturnedStatus = table.Column<int>(type: "int", nullable: false),
                    FactorId = table.Column<int>(type: "int", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnedFactors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnedFactors_Factors_FactorId",
                        column: x => x.FactorId,
                        principalTable: "Factors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FactorProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FactorId = table.Column<int>(type: "int", nullable: true),
                    ProductColorId = table.Column<int>(type: "int", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactorProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactorProducts_Factors_FactorId",
                        column: x => x.FactorId,
                        principalTable: "Factors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FactorProducts_ProductColors_ProductColorId",
                        column: x => x.ProductColorId,
                        principalTable: "ProductColors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LogReturnedFactors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnedFactorId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogReturnedFactors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogReturnedFactors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LogReturnedFactors_ReturnedFactors_ReturnedFactorId",
                        column: x => x.ReturnedFactorId,
                        principalTable: "ReturnedFactors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityId",
                table: "Addresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CityId",
                table: "AspNetUsers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_SubCategoryId",
                table: "Brands",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryDetails_FeatureId",
                table: "CategoryDetails",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryDetails_SubCategoryId",
                table: "CategoryDetails",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CatLandings_CategoryId",
                table: "CatLandings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_FactorProducts_FactorId",
                table: "FactorProducts",
                column: "FactorId");

            migrationBuilder.CreateIndex(
                name: "IX_FactorProducts_ProductColorId",
                table: "FactorProducts",
                column: "ProductColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_PostMethodId",
                table: "Factors",
                column: "PostMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_UserAddressId",
                table: "Factors",
                column: "UserAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_UserId",
                table: "Factors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageGalleries_ProductId",
                table: "ImageGalleries",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LogFactors_FactorId",
                table: "LogFactors",
                column: "FactorId");

            migrationBuilder.CreateIndex(
                name: "IX_LogFactors_UserId",
                table: "LogFactors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LogReturnedFactors_ReturnedFactorId",
                table: "LogReturnedFactors",
                column: "ReturnedFactorId");

            migrationBuilder.CreateIndex(
                name: "IX_LogReturnedFactors_UserId",
                table: "LogReturnedFactors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ColorId",
                table: "Offers",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ColorId",
                table: "ProductColors",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_GuaranteeId",
                table: "ProductColors",
                column: "GuaranteeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ProductId",
                table: "ProductColors",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_CategoryDetailId",
                table: "ProductDetails",
                column: "CategoryDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ProductId",
                table: "ProductDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OfferId",
                table: "Products",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubCategoryId",
                table: "Products",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedFactors_FactorId",
                table: "ReturnedFactors",
                column: "FactorId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategoryDetails_CategoryDetailId",
                table: "SubCategoryDetails",
                column: "CategoryDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategoryDetails_SubCategoryId",
                table: "SubCategoryDetails",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavs_ProductId",
                table: "UserFavs",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUsPages");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "CatLandings");

            migrationBuilder.DropTable(
                name: "ContactPages");

            migrationBuilder.DropTable(
                name: "ContactUs");

            migrationBuilder.DropTable(
                name: "DiscountCodes");

            migrationBuilder.DropTable(
                name: "FactorProducts");

            migrationBuilder.DropTable(
                name: "Faqs");

            migrationBuilder.DropTable(
                name: "FooterLinks");

            migrationBuilder.DropTable(
                name: "FooterPages");

            migrationBuilder.DropTable(
                name: "ImageGalleries");

            migrationBuilder.DropTable(
                name: "InventoryNotifications");

            migrationBuilder.DropTable(
                name: "LogFactors");

            migrationBuilder.DropTable(
                name: "LogReturnedFactors");

            migrationBuilder.DropTable(
                name: "ProductDetails");

            migrationBuilder.DropTable(
                name: "SaleServices");

            migrationBuilder.DropTable(
                name: "SearchResults");

            migrationBuilder.DropTable(
                name: "SeoPage");

            migrationBuilder.DropTable(
                name: "SubCategoryDetails");

            migrationBuilder.DropTable(
                name: "UserFavs");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ProductColors");

            migrationBuilder.DropTable(
                name: "ReturnedFactors");

            migrationBuilder.DropTable(
                name: "CategoryDetails");

            migrationBuilder.DropTable(
                name: "Guarantees");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Factors");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PostMethods");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "States");
        }
    }
}
