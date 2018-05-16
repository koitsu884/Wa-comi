using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class changedMainPhotoIdToUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainPhotoId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MainPhotoId",
                table: "BusinessUser");

            migrationBuilder.AddColumn<string>(
                name: "MainPhotoUrl",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainPhotoUrl",
                table: "ClanSeeks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainPhotoUrl",
                table: "BusinessUser",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainPhotoUrl",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MainPhotoUrl",
                table: "ClanSeeks");

            migrationBuilder.DropColumn(
                name: "MainPhotoUrl",
                table: "BusinessUser");

            migrationBuilder.AddColumn<int>(
                name: "MainPhotoId",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MainPhotoId",
                table: "BusinessUser",
                nullable: true);
        }
    }
}
