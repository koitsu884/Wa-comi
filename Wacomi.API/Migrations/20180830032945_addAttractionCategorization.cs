using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addAttractionCategorization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttractionCategories_Attractions_AttractionId",
                table: "AttractionCategories");

            migrationBuilder.DropIndex(
                name: "IX_AttractionCategories_AttractionId",
                table: "AttractionCategories");

            migrationBuilder.DropColumn(
                name: "AttractionId",
                table: "AttractionCategories");

            migrationBuilder.CreateTable(
                name: "AttractionCategorization",
                columns: table => new
                {
                    AttractionId = table.Column<int>(type: "int", nullable: false),
                    AttractionCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionCategorization", x => new { x.AttractionId, x.AttractionCategoryId });
                    table.ForeignKey(
                        name: "FK_AttractionCategorization_AttractionCategories_AttractionCategoryId",
                        column: x => x.AttractionCategoryId,
                        principalTable: "AttractionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttractionCategorization_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttractionCategorization_AttractionCategoryId",
                table: "AttractionCategorization",
                column: "AttractionCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionCategorization");

            migrationBuilder.AddColumn<int>(
                name: "AttractionId",
                table: "AttractionCategories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttractionCategories_AttractionId",
                table: "AttractionCategories",
                column: "AttractionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttractionCategories_Attractions_AttractionId",
                table: "AttractionCategories",
                column: "AttractionId",
                principalTable: "Attractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
