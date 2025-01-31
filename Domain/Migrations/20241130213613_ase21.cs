using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class ase21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParsBuyingGuideImage",
                table: "FooterPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParsConsultationBeforePurchaseDescImage",
                table: "FooterPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParsGoalsImage",
                table: "FooterPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParsInstallmentPurchaseImage",
                table: "FooterPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParsOnlineSupportDescImage",
                table: "FooterPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParsOrganizationalPurchaseImage",
                table: "FooterPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParsProceduresForReturningGoodsDescImage",
                table: "FooterPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParsRegulationsDescImage",
                table: "FooterPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParsShippingMethodsDescImage",
                table: "FooterPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParsTrackingOrdersDescImage",
                table: "FooterPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParsUserPrivacyDescImage",
                table: "FooterPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParsWarrantyDescImage",
                table: "FooterPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoParsDarYekImage",
                table: "FooterPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoWhyParsImage",
                table: "FooterPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParsBuyingGuideImage",
                table: "FooterPages");

            migrationBuilder.DropColumn(
                name: "ParsConsultationBeforePurchaseDescImage",
                table: "FooterPages");

            migrationBuilder.DropColumn(
                name: "ParsGoalsImage",
                table: "FooterPages");

            migrationBuilder.DropColumn(
                name: "ParsInstallmentPurchaseImage",
                table: "FooterPages");

            migrationBuilder.DropColumn(
                name: "ParsOnlineSupportDescImage",
                table: "FooterPages");

            migrationBuilder.DropColumn(
                name: "ParsOrganizationalPurchaseImage",
                table: "FooterPages");

            migrationBuilder.DropColumn(
                name: "ParsProceduresForReturningGoodsDescImage",
                table: "FooterPages");

            migrationBuilder.DropColumn(
                name: "ParsRegulationsDescImage",
                table: "FooterPages");

            migrationBuilder.DropColumn(
                name: "ParsShippingMethodsDescImage",
                table: "FooterPages");

            migrationBuilder.DropColumn(
                name: "ParsTrackingOrdersDescImage",
                table: "FooterPages");

            migrationBuilder.DropColumn(
                name: "ParsUserPrivacyDescImage",
                table: "FooterPages");

            migrationBuilder.DropColumn(
                name: "ParsWarrantyDescImage",
                table: "FooterPages");

            migrationBuilder.DropColumn(
                name: "SeoParsDarYekImage",
                table: "FooterPages");

            migrationBuilder.DropColumn(
                name: "SeoWhyParsImage",
                table: "FooterPages");
        }
    }
}
