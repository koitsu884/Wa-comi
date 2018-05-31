using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class someChangesOnTopicCommentAndReply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicComments_Members_MemberId",
                table: "TopicComments");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "TopicComments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "TopicComments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "MainPhotoUrl",
                table: "TopicComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TopicReplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainPhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberId = table.Column<int>(type: "int", nullable: true),
                    Reply = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TopicCommentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicReplies_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TopicReplies_TopicComments_TopicCommentId",
                        column: x => x.TopicCommentId,
                        principalTable: "TopicComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopicReplies_MemberId",
                table: "TopicReplies",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicReplies_TopicCommentId",
                table: "TopicReplies",
                column: "TopicCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicComments_Members_MemberId",
                table: "TopicComments",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicComments_Members_MemberId",
                table: "TopicComments");

            migrationBuilder.DropTable(
                name: "TopicReplies");

            migrationBuilder.DropColumn(
                name: "MainPhotoUrl",
                table: "TopicComments");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "TopicComments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "TopicComments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicComments_Members_MemberId",
                table: "TopicComments",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
