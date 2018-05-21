using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addLocationIdToSeeks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "PropertySeeks");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ClanSeeks");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "PropertySeeks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "ClanSeeks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PropertySeeks_LocationId",
                table: "PropertySeeks",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClanSeeks_LocationId",
                table: "ClanSeeks",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClanSeeks_Cities_LocationId",
                table: "ClanSeeks",
                column: "LocationId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertySeeks_Cities_LocationId",
                table: "PropertySeeks",
                column: "LocationId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClanSeeks_Cities_LocationId",
                table: "ClanSeeks");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertySeeks_Cities_LocationId",
                table: "PropertySeeks");

            migrationBuilder.DropIndex(
                name: "IX_PropertySeeks_LocationId",
                table: "PropertySeeks");

            migrationBuilder.DropIndex(
                name: "IX_ClanSeeks_LocationId",
                table: "ClanSeeks");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "PropertySeeks");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "ClanSeeks");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "PropertySeeks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ClanSeeks",
                nullable: true);
        }
    }
}
