using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class madeSomeShangesForDataStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Circles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Circles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Circles_AppUserId",
                table: "Circles",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Circles_AppUsers_AppUserId",
                table: "Circles",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Circles_AppUsers_AppUserId",
                table: "Circles");

            migrationBuilder.DropIndex(
                name: "IX_Circles_AppUserId",
                table: "Circles");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Circles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Circles");
        }
    }
}
