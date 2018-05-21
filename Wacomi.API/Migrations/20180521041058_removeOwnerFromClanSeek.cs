using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class removeOwnerFromClanSeek : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClanSeeks_Members_OwnerId",
                table: "ClanSeeks");

            migrationBuilder.DropIndex(
                name: "IX_ClanSeeks_OwnerId",
                table: "ClanSeeks");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ClanSeeks");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "ClanSeeks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClanSeeks_MemberId",
                table: "ClanSeeks",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClanSeeks_Members_MemberId",
                table: "ClanSeeks",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClanSeeks_Members_MemberId",
                table: "ClanSeeks");

            migrationBuilder.DropIndex(
                name: "IX_ClanSeeks_MemberId",
                table: "ClanSeeks");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "ClanSeeks");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "ClanSeeks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClanSeeks_OwnerId",
                table: "ClanSeeks",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClanSeeks_Members_OwnerId",
                table: "ClanSeeks",
                column: "OwnerId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
