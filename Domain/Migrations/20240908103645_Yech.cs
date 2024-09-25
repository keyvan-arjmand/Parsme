using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class Yech : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title3Col3",
                table: "FooterLinks",
                newName: "TitleCol4");

            migrationBuilder.RenameColumn(
                name: "Href3Col3",
                table: "FooterLinks",
                newName: "Title5Col2");

            migrationBuilder.AddColumn<string>(
                name: "DescCol4",
                table: "FooterLinks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Href1Col4",
                table: "FooterLinks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Href4Col1",
                table: "FooterLinks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Href4Col2",
                table: "FooterLinks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Href5Col2",
                table: "FooterLinks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HrefCol4",
                table: "FooterLinks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title1Col4",
                table: "FooterLinks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title4Col1",
                table: "FooterLinks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title4Col2",
                table: "FooterLinks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescCol4",
                table: "FooterLinks");

            migrationBuilder.DropColumn(
                name: "Href1Col4",
                table: "FooterLinks");

            migrationBuilder.DropColumn(
                name: "Href4Col1",
                table: "FooterLinks");

            migrationBuilder.DropColumn(
                name: "Href4Col2",
                table: "FooterLinks");

            migrationBuilder.DropColumn(
                name: "Href5Col2",
                table: "FooterLinks");

            migrationBuilder.DropColumn(
                name: "HrefCol4",
                table: "FooterLinks");

            migrationBuilder.DropColumn(
                name: "Title1Col4",
                table: "FooterLinks");

            migrationBuilder.DropColumn(
                name: "Title4Col1",
                table: "FooterLinks");

            migrationBuilder.DropColumn(
                name: "Title4Col2",
                table: "FooterLinks");

            migrationBuilder.RenameColumn(
                name: "TitleCol4",
                table: "FooterLinks",
                newName: "Title3Col3");

            migrationBuilder.RenameColumn(
                name: "Title5Col2",
                table: "FooterLinks",
                newName: "Href3Col3");
        }
    }
}
