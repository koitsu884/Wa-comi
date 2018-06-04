using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class FixForeignKeyOfTopicCommentFeel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicCommentFeels_Members_CommentId",
                table: "TopicCommentFeels");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicCommentFeels_Members_MemberId",
                table: "TopicCommentFeels",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicCommentFeels_Members_MemberId",
                table: "TopicCommentFeels");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicCommentFeels_Members_CommentId",
                table: "TopicCommentFeels",
                column: "CommentId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
