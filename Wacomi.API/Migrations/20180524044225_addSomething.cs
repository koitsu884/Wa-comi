using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addSomething : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicLike_DailyTopics_DairyTopicId",
                table: "TopicLike");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicLike_AspNetUsers_SupportUserId",
                table: "TopicLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicLike",
                table: "TopicLike");

            migrationBuilder.RenameTable(
                name: "TopicLike",
                newName: "TopicLikes");

            migrationBuilder.RenameIndex(
                name: "IX_TopicLike_DairyTopicId",
                table: "TopicLikes",
                newName: "IX_TopicLikes_DairyTopicId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicLikes",
                table: "TopicLikes",
                columns: new[] { "SupportUserId", "DairyTopicId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TopicLikes_DailyTopics_DairyTopicId",
                table: "TopicLikes",
                column: "DairyTopicId",
                principalTable: "DailyTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicLikes_AspNetUsers_SupportUserId",
                table: "TopicLikes",
                column: "SupportUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicLikes_DailyTopics_DairyTopicId",
                table: "TopicLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicLikes_AspNetUsers_SupportUserId",
                table: "TopicLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicLikes",
                table: "TopicLikes");

            migrationBuilder.RenameTable(
                name: "TopicLikes",
                newName: "TopicLike");

            migrationBuilder.RenameIndex(
                name: "IX_TopicLikes_DairyTopicId",
                table: "TopicLike",
                newName: "IX_TopicLike_DairyTopicId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicLike",
                table: "TopicLike",
                columns: new[] { "SupportUserId", "DairyTopicId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TopicLike_DailyTopics_DairyTopicId",
                table: "TopicLike",
                column: "DairyTopicId",
                principalTable: "DailyTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicLike_AspNetUsers_SupportUserId",
                table: "TopicLike",
                column: "SupportUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
