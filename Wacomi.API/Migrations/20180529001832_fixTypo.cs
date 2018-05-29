using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class fixTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicLikes_DailyTopics_DairyTopicId",
                table: "TopicLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicLikes",
                table: "TopicLikes");

            migrationBuilder.DropIndex(
                name: "IX_TopicLikes_DairyTopicId",
                table: "TopicLikes");

            migrationBuilder.DropColumn(
                name: "DairyTopicId",
                table: "TopicLikes");

            migrationBuilder.AddColumn<int>(
                name: "DailyTopicId",
                table: "TopicLikes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicLikes",
                table: "TopicLikes",
                columns: new[] { "SupportUserId", "DailyTopicId" });

            migrationBuilder.CreateIndex(
                name: "IX_TopicLikes_DailyTopicId",
                table: "TopicLikes",
                column: "DailyTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicLikes_DailyTopics_DailyTopicId",
                table: "TopicLikes",
                column: "DailyTopicId",
                principalTable: "DailyTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicLikes_DailyTopics_DailyTopicId",
                table: "TopicLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicLikes",
                table: "TopicLikes");

            migrationBuilder.DropIndex(
                name: "IX_TopicLikes_DailyTopicId",
                table: "TopicLikes");

            migrationBuilder.DropColumn(
                name: "DailyTopicId",
                table: "TopicLikes");

            migrationBuilder.AddColumn<int>(
                name: "DairyTopicId",
                table: "TopicLikes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicLikes",
                table: "TopicLikes",
                columns: new[] { "SupportUserId", "DairyTopicId" });

            migrationBuilder.CreateIndex(
                name: "IX_TopicLikes_DairyTopicId",
                table: "TopicLikes",
                column: "DairyTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicLikes_DailyTopics_DairyTopicId",
                table: "TopicLikes",
                column: "DairyTopicId",
                principalTable: "DailyTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
