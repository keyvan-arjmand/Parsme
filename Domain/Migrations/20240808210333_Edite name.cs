using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class Editename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Href4Col3",
                table: "FooterLinks",
                newName: "Href3Col3");

            migrationBuilder.RenameColumn(
                name: "Href4Col2",
                table: "FooterLinks",
                newName: "Href3Col2");

            migrationBuilder.RenameColumn(
                name: "Href4Col1",
                table: "FooterLinks",
                newName: "Href3Col1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Href3Col3",
                table: "FooterLinks",
                newName: "Href4Col3");

            migrationBuilder.RenameColumn(
                name: "Href3Col2",
                table: "FooterLinks",
                newName: "Href4Col2");

            migrationBuilder.RenameColumn(
                name: "Href3Col1",
                table: "FooterLinks",
                newName: "Href4Col1");
        }
    }
}
