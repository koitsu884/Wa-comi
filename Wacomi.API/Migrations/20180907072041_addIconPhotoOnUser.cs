using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addIconPhotoOnUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IconPhotoId",
                table: "AppUsers",
                type: "int",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
