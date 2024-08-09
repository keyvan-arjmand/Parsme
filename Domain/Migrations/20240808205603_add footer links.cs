using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addfooterlinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Href4Col1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescCol2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleCol2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefCol2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title1Col2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href1Col2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title2Col2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href2Col2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title3Col2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href4Col2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescCol3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleCol3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HrefCol23 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title1Col3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href1Col3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title2Col3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href2Col3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title3Col3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href4Col3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescFooter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterLinks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FooterLinks");
        }
    }
}
