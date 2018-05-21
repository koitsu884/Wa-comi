using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addMemberToClanSeek : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClanSeeks_Members_MemberId",
                table: "ClanSeeks");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "ClanSeeks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClanSeeks_Members_MemberId",
                table: "ClanSeeks",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClanSeeks_Members_MemberId",
                table: "ClanSeeks");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "ClanSeeks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ClanSeeks_Members_MemberId",
                table: "ClanSeeks",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
