using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class FooterPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FooterPages");
        }
    }
}
