using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addDateColumnOnCircleEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "CircleEvents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_CircleEvents_CircleId",
                table: "CircleEvents",
                column: "CircleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CircleEvents_Circles_CircleId",
                table: "CircleEvents",
                column: "CircleId",
                principalTable: "Circles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CircleEvents_Circles_CircleId",
                table: "CircleEvents");

            migrationBuilder.DropIndex(
                name: "IX_CircleEvents_CircleId",
                table: "CircleEvents");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "CircleEvents");
        }
    }
}
