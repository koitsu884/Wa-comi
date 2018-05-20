using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addMainPhotoUrlToBlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Photos_PhotoId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_PhotoId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Blogs");

            migrationBuilder.AddColumn<string>(
                name: "BlogImageUrl",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlogImageUrl",
                table: "Blogs");

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Blogs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_PhotoId",
                table: "Blogs",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Photos_PhotoId",
                table: "Blogs",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
