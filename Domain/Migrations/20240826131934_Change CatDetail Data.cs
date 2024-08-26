using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCatDetailData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDetails_SubCategories_SubCategoryId",
                table: "CategoryDetails");

            migrationBuilder.AlterColumn<int>(
                name: "SubCategoryId",
                table: "CategoryDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.CreateIndex(
                name: "IX_SubCategoryDetails_CategoryDetailId",
                table: "SubCategoryDetails",
                column: "CategoryDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategoryDetails_SubCategoryId",
                table: "SubCategoryDetails",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDetails_SubCategories_SubCategoryId",
                table: "CategoryDetails",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDetails_SubCategories_SubCategoryId",
                table: "CategoryDetails");

            migrationBuilder.DropTable(
                name: "SubCategoryDetails");

            migrationBuilder.AlterColumn<int>(
                name: "SubCategoryId",
                table: "CategoryDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDetails_SubCategories_SubCategoryId",
                table: "CategoryDetails",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
