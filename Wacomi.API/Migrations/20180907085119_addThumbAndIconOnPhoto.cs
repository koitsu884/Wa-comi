using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addThumbAndIconOnPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Photos_IconPhotoId",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_IconPhotoId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "IconPhotoId",
                table: "AppUsers");

            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                table: "Photos",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Photos",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconUrl",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Photos");

            migrationBuilder.AddColumn<int>(
                name: "IconPhotoId",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_IconPhotoId",
                table: "AppUsers",
                column: "IconPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Photos_IconPhotoId",
                table: "AppUsers",
                column: "IconPhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
