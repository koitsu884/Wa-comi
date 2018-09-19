using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class bitUpdatesForCircle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Circles_CircleCategory_CategoryId",
                table: "Circles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CircleCategory",
                table: "CircleCategory");

            migrationBuilder.RenameTable(
                name: "CircleCategory",
                newName: "CircleCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CircleCategories",
                table: "CircleCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Circles_CircleCategories_CategoryId",
                table: "Circles",
                column: "CategoryId",
                principalTable: "CircleCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Circles_CircleCategories_CategoryId",
                table: "Circles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CircleCategories",
                table: "CircleCategories");

            migrationBuilder.RenameTable(
                name: "CircleCategories",
                newName: "CircleCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CircleCategory",
                table: "CircleCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Circles_CircleCategory_CategoryId",
                table: "Circles",
                column: "CategoryId",
                principalTable: "CircleCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
