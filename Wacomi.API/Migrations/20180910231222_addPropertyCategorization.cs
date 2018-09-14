using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addPropertyCategorization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertySeekCategories_Properties_PropertyId",
                table: "PropertySeekCategories");

            migrationBuilder.DropIndex(
                name: "IX_PropertySeekCategories_PropertyId",
                table: "PropertySeekCategories");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "PropertySeekCategories");

            migrationBuilder.CreateTable(
                name: "PropertyCategorization",
                columns: table => new
                {
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    PropertySeekCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyCategorization", x => new { x.PropertyId, x.PropertySeekCategoryId });
                    table.ForeignKey(
                        name: "FK_PropertyCategorization_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyCategorization_PropertySeekCategories_PropertySeekCategoryId",
                        column: x => x.PropertySeekCategoryId,
                        principalTable: "PropertySeekCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Capacity",
                table: "Properties",
                column: "Capacity");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_DateAvailable",
                table: "Properties",
                column: "DateAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_HasChild",
                table: "Properties",
                column: "HasChild");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_HasPet",
                table: "Properties",
                column: "HasPet");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Internet",
                table: "Properties",
                column: "Internet");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_IsActive",
                table: "Properties",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Latitude",
                table: "Properties",
                column: "Latitude");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Longitude",
                table: "Properties",
                column: "Longitude");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_MaxTerm",
                table: "Properties",
                column: "MaxTerm");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_MinTerm",
                table: "Properties",
                column: "MinTerm");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Rent",
                table: "Properties",
                column: "Rent");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyCategorization_PropertySeekCategoryId",
                table: "PropertyCategorization",
                column: "PropertySeekCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyCategorization");

            migrationBuilder.DropIndex(
                name: "IX_Properties_Capacity",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_DateAvailable",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_HasChild",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_HasPet",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_Internet",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_IsActive",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_Latitude",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_Longitude",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_MaxTerm",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_MinTerm",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_Rent",
                table: "Properties");

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "PropertySeekCategories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertySeekCategories_PropertyId",
                table: "PropertySeekCategories",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertySeekCategories_Properties_PropertyId",
                table: "PropertySeekCategories",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
