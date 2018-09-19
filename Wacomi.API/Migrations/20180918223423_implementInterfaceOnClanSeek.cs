using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class implementInterfaceOnClanSeek : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_AppUsers_AppUserId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_ClanSeeks_Created",
                table: "ClanSeeks");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ClanSeeks");

            migrationBuilder.DropColumn(
                name: "LastActive",
                table: "ClanSeeks");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Properties",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ClanSeeks",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "ClanSeeks",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ClanSeeks_DateCreated",
                table: "ClanSeeks",
                column: "DateCreated");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_AppUsers_AppUserId",
                table: "Properties",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_AppUsers_AppUserId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_ClanSeeks_DateCreated",
                table: "ClanSeeks");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ClanSeeks");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "ClanSeeks");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Properties",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ClanSeeks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActive",
                table: "ClanSeeks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ClanSeeks_Created",
                table: "ClanSeeks",
                column: "Created");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_AppUsers_AppUserId",
                table: "Properties",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
