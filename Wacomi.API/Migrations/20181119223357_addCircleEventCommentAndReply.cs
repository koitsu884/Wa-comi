using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addCircleEventCommentAndReply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CircleEventComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(nullable: true),
                    CircleEventId = table.Column<int>(nullable: false),
                    CircleId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(maxLength: 2000, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircleEventComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CircleEventComments_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CircleEventComments_CircleEvents_CircleEventId",
                        column: x => x.CircleEventId,
                        principalTable: "CircleEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CircleEventCommentReplies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(nullable: true),
                    CircleId = table.Column<int>(nullable: false),
                    CommentId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Reply = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircleEventCommentReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CircleEventCommentReplies_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CircleEventCommentReplies_CircleEventComments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "CircleEventComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CircleEventCommentReplies_AppUserId",
                table: "CircleEventCommentReplies",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleEventCommentReplies_CommentId",
                table: "CircleEventCommentReplies",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleEventComments_AppUserId",
                table: "CircleEventComments",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleEventComments_CircleEventId",
                table: "CircleEventComments",
                column: "CircleEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CircleEventCommentReplies");

            migrationBuilder.DropTable(
                name: "CircleEventComments");
        }
    }
}
