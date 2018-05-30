using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class changeTopicCommentToStoreMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicComments_AspNetUsers_UserId1",
                table: "TopicComments");

            migrationBuilder.DropIndex(
                name: "IX_TopicComments_UserId1",
                table: "TopicComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TopicComments");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "TopicComments");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "TopicComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TopicComments_MemberId",
                table: "TopicComments",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicComments_Members_MemberId",
                table: "TopicComments",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicComments_Members_MemberId",
                table: "TopicComments");

            migrationBuilder.DropIndex(
                name: "IX_TopicComments_MemberId",
                table: "TopicComments");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "TopicComments");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TopicComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "TopicComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopicComments_UserId1",
                table: "TopicComments",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicComments_AspNetUsers_UserId1",
                table: "TopicComments",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
