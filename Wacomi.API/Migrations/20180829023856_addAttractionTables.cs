using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addAttractionTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttractionId",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Attractions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccessInfo = table.Column<string>(type: "longtext", nullable: true),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Introduction = table.Column<string>(type: "varchar(1500)", maxLength: 1500, nullable: false),
                    Latitude = table.Column<double>(type: "double", nullable: false),
                    Longitude = table.Column<double>(type: "double", nullable: false),
                    MainPhotoId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    WebsiteUrl = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attractions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attractions_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attractions_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attractions_Photos_MainPhotoId",
                        column: x => x.MainPhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttractionCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AttractionId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttractionCategories_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttractionLikes",
                columns: table => new
                {
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    AttractionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionLikes", x => new { x.AppUserId, x.AttractionId });
                    table.ForeignKey(
                        name: "FK_AttractionLikes_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttractionLikes_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttractionReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    AttractionId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttractionReviews_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttractionReviews_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttractionReviewLikes",
                columns: table => new
                {
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    AttractionReviewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionReviewLikes", x => new { x.AppUserId, x.AttractionReviewId });
                    table.ForeignKey(
                        name: "FK_AttractionReviewLikes_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttractionReviewLikes_AttractionReviews_AttractionReviewId",
                        column: x => x.AttractionReviewId,
                        principalTable: "AttractionReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AttractionId",
                table: "Photos",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionCategories_AttractionId",
                table: "AttractionCategories",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionLikes_AttractionId",
                table: "AttractionLikes",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionReviewLikes_AttractionReviewId",
                table: "AttractionReviewLikes",
                column: "AttractionReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionReviews_AppUserId",
                table: "AttractionReviews",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionReviews_AttractionId",
                table: "AttractionReviews",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Attractions_AppUserId",
                table: "Attractions",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Attractions_CityId",
                table: "Attractions",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Attractions_MainPhotoId",
                table: "Attractions",
                column: "MainPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Attractions_AttractionId",
                table: "Photos",
                column: "AttractionId",
                principalTable: "Attractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Attractions_AttractionId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "AttractionCategories");

            migrationBuilder.DropTable(
                name: "AttractionLikes");

            migrationBuilder.DropTable(
                name: "AttractionReviewLikes");

            migrationBuilder.DropTable(
                name: "AttractionReviews");

            migrationBuilder.DropTable(
                name: "Attractions");

            migrationBuilder.DropIndex(
                name: "IX_Photos_AttractionId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "AttractionId",
                table: "Photos");
        }
    }
}
