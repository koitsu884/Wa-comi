using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addFeedLikesAndComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogFeedComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    BlogFeedId = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainPhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFeedComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogFeedComments_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlogFeedComments_BlogFeeds_BlogFeedId",
                        column: x => x.BlogFeedId,
                        principalTable: "BlogFeeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlogFeedLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BlogFeedId = table.Column<int>(type: "int", nullable: true),
                    SupportAppUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFeedLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogFeedLikes_BlogFeeds_BlogFeedId",
                        column: x => x.BlogFeedId,
                        principalTable: "BlogFeeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlogFeedLikes_AppUsers_SupportAppUserId",
                        column: x => x.SupportAppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogFeeds_PublishingDate",
                table: "BlogFeeds",
                column: "PublishingDate");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFeedComments_AppUserId",
                table: "BlogFeedComments",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFeedComments_BlogFeedId",
                table: "BlogFeedComments",
                column: "BlogFeedId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFeedLikes_BlogFeedId",
                table: "BlogFeedLikes",
                column: "BlogFeedId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFeedLikes_SupportAppUserId",
                table: "BlogFeedLikes",
                column: "SupportAppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogFeedComments");

            migrationBuilder.DropTable(
                name: "BlogFeedLikes");

            migrationBuilder.DropIndex(
                name: "IX_BlogFeeds_PublishingDate",
                table: "BlogFeeds");
        }
    }
}
