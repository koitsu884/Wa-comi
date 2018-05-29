using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addDailyTopicStructureChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicComments_Members_MemberId",
                table: "TopicComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicLike_Members_DairyTopicId",
                table: "TopicLike");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicLike_DailyTopics_SupportMemberId",
                table: "TopicLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicLike",
                table: "TopicLike");

            migrationBuilder.DropIndex(
                name: "IX_TopicComments_MemberId",
                table: "TopicComments");

            migrationBuilder.DropColumn(
                name: "SupportMemberId",
                table: "TopicLike");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "TopicComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DailyTopics");

            migrationBuilder.AddColumn<string>(
                name: "SupportUserId",
                table: "TopicLike",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "TopicComments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "TopicTitle",
                table: "TopicComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TopicComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "TopicComments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTemporary",
                table: "DailyTopics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DailyTopics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ClanSeeks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicLike",
                table: "TopicLike",
                columns: new[] { "SupportUserId", "DairyTopicId" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicComments_AspNetUsers_UserId1",
                table: "TopicComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicLike_DailyTopics_DairyTopicId",
                table: "TopicLike");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicLike_AspNetUsers_SupportUserId",
                table: "TopicLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicLike",
                table: "TopicLike");

            migrationBuilder.DropIndex(
                name: "IX_TopicComments_UserId1",
                table: "TopicComments");

            migrationBuilder.DropColumn(
                name: "SupportUserId",
                table: "TopicLike");

            migrationBuilder.DropColumn(
                name: "TopicTitle",
                table: "TopicComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TopicComments");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "TopicComments");

            migrationBuilder.DropColumn(
                name: "IsTemporary",
                table: "DailyTopics");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DailyTopics");

            migrationBuilder.AddColumn<int>(
                name: "SupportMemberId",
                table: "TopicLike",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "TopicComments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "TopicComments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DailyTopics",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ClanSeeks",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicLike",
                table: "TopicLike",
                columns: new[] { "SupportMemberId", "DairyTopicId" });

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
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicLike_Members_DairyTopicId",
                table: "TopicLike",
                column: "DairyTopicId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicLike_DailyTopics_SupportMemberId",
                table: "TopicLike",
                column: "SupportMemberId",
                principalTable: "DailyTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
