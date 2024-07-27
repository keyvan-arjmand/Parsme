using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addfactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "FactorProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FactorId = table.Column<int>(type: "int", nullable: true),
                    ProductColorId = table.Column<int>(type: "int", nullable: true),
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
                name: "IX_ReturnedFactors_FactorId",
                table: "ReturnedFactors",
                column: "FactorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FactorProducts");

            migrationBuilder.DropTable(
                name: "LogFactors");

            migrationBuilder.DropTable(
                name: "LogReturnedFactors");

            migrationBuilder.DropTable(
                name: "ReturnedFactors");

            migrationBuilder.DropTable(
                name: "Factors");
        }
    }
}
