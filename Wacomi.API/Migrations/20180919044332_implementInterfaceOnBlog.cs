using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class implementInterfaceOnBlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Blogs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "Blogs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "Blogs");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Circles",
                nullable: true);

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
    }
}
