using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class ChangeBlogToArray : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessUser_Blogs_BlogId",
                table: "BusinessUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Blogs_BlogId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_BlogId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_BusinessUser_BlogId",
                table: "BusinessUser");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "BusinessUser");

            migrationBuilder.AddColumn<bool>(
                name: "IsPremium",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPremium",
                table: "BusinessUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BusinessUserId",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BusinessUserId",
                table: "Blogs",
                column: "BusinessUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_MemberId",
                table: "Blogs",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BusinessUser_BusinessUserId",
                table: "Blogs",
                column: "BusinessUserId",
                principalTable: "BusinessUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Members_MemberId",
                table: "Blogs",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BusinessUser_BusinessUserId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Members_MemberId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_BusinessUserId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_MemberId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "IsPremium",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "IsPremium",
                table: "BusinessUser");

            migrationBuilder.DropColumn(
                name: "BusinessUserId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Blogs");

            migrationBuilder.AddColumn<int>(
                name: "BlogId",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BlogId",
                table: "BusinessUser",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_BlogId",
                table: "Members",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUser_BlogId",
                table: "BusinessUser",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessUser_Blogs_BlogId",
                table: "BusinessUser",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Blogs_BlogId",
                table: "Members",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
