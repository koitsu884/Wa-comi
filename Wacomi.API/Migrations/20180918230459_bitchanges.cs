using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class bitchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClanSeeks_AppUsers_AppUserId",
                table: "ClanSeeks");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "ClanSeeks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ClanSeeks_AppUsers_AppUserId",
                table: "ClanSeeks",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClanSeeks_AppUsers_AppUserId",
                table: "ClanSeeks");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "ClanSeeks",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClanSeeks_AppUsers_AppUserId",
                table: "ClanSeeks",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
