using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addCircleRelatingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CircleId",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CircleCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircleCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Circles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: false),
                    MainPhotoId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Circles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Circles_CircleCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CircleCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Circles_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Circles_Photos_MainPhotoId",
                        column: x => x.MainPhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CircleMembers",
                columns: table => new
                {
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    CircleId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircleMembers", x => new { x.AppUserId, x.CircleId });
                    table.ForeignKey(
                        name: "FK_CircleMembers_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CircleMembers_Circles_CircleId",
                        column: x => x.CircleId,
                        principalTable: "Circles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CircleTopic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    CircleId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: false),
                    PhotoId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircleTopic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CircleTopic_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CircleTopic_Circles_CircleId",
                        column: x => x.CircleId,
                        principalTable: "Circles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CircleTopic_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CircleTopicComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    CircleTopicId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PhotoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircleTopicComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CircleTopicComments_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CircleTopicComments_CircleTopic_CircleTopicId",
                        column: x => x.CircleTopicId,
                        principalTable: "CircleTopic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CircleTopicComments_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CircleTopicCommentReplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Reply = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircleTopicCommentReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CircleTopicCommentReplies_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CircleTopicCommentReplies_CircleTopicComments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "CircleTopicComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_CircleId",
                table: "Photos",
                column: "CircleId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleMembers_CircleId",
                table: "CircleMembers",
                column: "CircleId");

            migrationBuilder.CreateIndex(
                name: "IX_Circles_CategoryId",
                table: "Circles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Circles_CityId",
                table: "Circles",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Circles_MainPhotoId",
                table: "Circles",
                column: "MainPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleTopic_AppUserId",
                table: "CircleTopic",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleTopic_CircleId",
                table: "CircleTopic",
                column: "CircleId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleTopic_PhotoId",
                table: "CircleTopic",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleTopicCommentReplies_AppUserId",
                table: "CircleTopicCommentReplies",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleTopicCommentReplies_CommentId",
                table: "CircleTopicCommentReplies",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleTopicComments_AppUserId",
                table: "CircleTopicComments",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleTopicComments_CircleTopicId",
                table: "CircleTopicComments",
                column: "CircleTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleTopicComments_PhotoId",
                table: "CircleTopicComments",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Circles_CircleId",
                table: "Photos",
                column: "CircleId",
                principalTable: "Circles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Circles_CircleId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "CircleMembers");

            migrationBuilder.DropTable(
                name: "CircleTopicCommentReplies");

            migrationBuilder.DropTable(
                name: "CircleTopicComments");

            migrationBuilder.DropTable(
                name: "CircleTopic");

            migrationBuilder.DropTable(
                name: "Circles");

            migrationBuilder.DropTable(
                name: "CircleCategory");

            migrationBuilder.DropIndex(
                name: "IX_Photos_CircleId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "CircleId",
                table: "Photos");
        }
    }
}
