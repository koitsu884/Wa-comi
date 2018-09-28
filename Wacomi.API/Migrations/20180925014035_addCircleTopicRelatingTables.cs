using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addCircleTopicRelatingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CircleTopic_AppUsers_AppUserId",
                table: "CircleTopic");

            migrationBuilder.DropForeignKey(
                name: "FK_CircleTopic_Circles_CircleId",
                table: "CircleTopic");

            migrationBuilder.DropForeignKey(
                name: "FK_CircleTopic_Photos_PhotoId",
                table: "CircleTopic");

            migrationBuilder.DropForeignKey(
                name: "FK_CircleTopicComments_CircleTopic_CircleTopicId",
                table: "CircleTopicComments");

            migrationBuilder.DropForeignKey(
                name: "FK_CircleTopicComments_Photos_PhotoId",
                table: "CircleTopicComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CircleTopic",
                table: "CircleTopic");

            migrationBuilder.RenameTable(
                name: "CircleTopic",
                newName: "CircleTopics");

            migrationBuilder.RenameIndex(
                name: "IX_CircleTopic_PhotoId",
                table: "CircleTopics",
                newName: "IX_CircleTopics_PhotoId");

            migrationBuilder.RenameIndex(
                name: "IX_CircleTopic_CircleId",
                table: "CircleTopics",
                newName: "IX_CircleTopics_CircleId");

            migrationBuilder.RenameIndex(
                name: "IX_CircleTopic_AppUserId",
                table: "CircleTopics",
                newName: "IX_CircleTopics_AppUserId");

            migrationBuilder.AlterColumn<int>(
                name: "PhotoId",
                table: "CircleTopicComments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CircleId",
                table: "CircleTopicComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CircleTopicComments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CircleId",
                table: "CircleTopicCommentReplies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CircleTopicCommentReplies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "PhotoId",
                table: "CircleTopics",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CircleTopics",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 5000);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CircleTopics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSecret",
                table: "CircleTopics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CircleTopics",
                table: "CircleTopics",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CircleTopicComments_CircleTopics_CircleTopicId",
                table: "CircleTopicComments",
                column: "CircleTopicId",
                principalTable: "CircleTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CircleTopicComments_Photos_PhotoId",
                table: "CircleTopicComments",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CircleTopics_AppUsers_AppUserId",
                table: "CircleTopics",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CircleTopics_Circles_CircleId",
                table: "CircleTopics",
                column: "CircleId",
                principalTable: "Circles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CircleTopics_Photos_PhotoId",
                table: "CircleTopics",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CircleTopicComments_CircleTopics_CircleTopicId",
                table: "CircleTopicComments");

            migrationBuilder.DropForeignKey(
                name: "FK_CircleTopicComments_Photos_PhotoId",
                table: "CircleTopicComments");

            migrationBuilder.DropForeignKey(
                name: "FK_CircleTopics_AppUsers_AppUserId",
                table: "CircleTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_CircleTopics_Circles_CircleId",
                table: "CircleTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_CircleTopics_Photos_PhotoId",
                table: "CircleTopics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CircleTopics",
                table: "CircleTopics");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CircleTopics");

            migrationBuilder.DropColumn(
                name: "IsSecret",
                table: "CircleTopics");

            migrationBuilder.DropColumn(
                name: "CircleId",
                table: "CircleTopicComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CircleTopicComments");

            migrationBuilder.DropColumn(
                name: "CircleId",
                table: "CircleTopicCommentReplies");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CircleTopicCommentReplies");

            migrationBuilder.RenameTable(
                name: "CircleTopics",
                newName: "CircleTopic");

            migrationBuilder.RenameIndex(
                name: "IX_CircleTopics_PhotoId",
                table: "CircleTopic",
                newName: "IX_CircleTopic_PhotoId");

            migrationBuilder.RenameIndex(
                name: "IX_CircleTopics_CircleId",
                table: "CircleTopic",
                newName: "IX_CircleTopic_CircleId");

            migrationBuilder.RenameIndex(
                name: "IX_CircleTopics_AppUserId",
                table: "CircleTopic",
                newName: "IX_CircleTopic_AppUserId");

            migrationBuilder.AlterColumn<int>(
                name: "PhotoId",
                table: "CircleTopic",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CircleTopic",
                maxLength: 5000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<int>(
                name: "PhotoId",
                table: "CircleTopicComments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CircleTopic",
                table: "CircleTopic",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CircleTopic_AppUsers_AppUserId",
                table: "CircleTopic",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CircleTopic_Circles_CircleId",
                table: "CircleTopic",
                column: "CircleId",
                principalTable: "Circles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CircleTopic_Photos_PhotoId",
                table: "CircleTopic",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CircleTopicComments_CircleTopic_CircleTopicId",
                table: "CircleTopicComments",
                column: "CircleTopicId",
                principalTable: "CircleTopic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CircleTopicComments_Photos_PhotoId",
                table: "CircleTopicComments",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
