using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class Missing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleServices");
        }
    }
}
