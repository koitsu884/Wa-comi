using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class implementInterfaceOnAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClanSeeks_AppUsers_AppUserId",
                table: "ClanSeeks");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_AppUsers_AppUserId",
                table: "Properties");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Properties",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "ClanSeeks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "AppUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_ClanSeeks_AppUsers_AppUserId",
                table: "ClanSeeks",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_AppUsers_AppUserId",
                table: "Properties",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClanSeeks_AppUsers_AppUserId",
                table: "ClanSeeks");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_AppUsers_AppUserId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "AppUsers");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Properties",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "ClanSeeks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ClanSeeks_AppUsers_AppUserId",
                table: "ClanSeeks",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_AppUsers_AppUserId",
                table: "Properties",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
