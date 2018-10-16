using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addCircleEventTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CircleEventId",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CircleEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(type: "longtext", nullable: true),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    ApprovalRequired = table.Column<bool>(type: "bit", nullable: false),
                    CircleId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    MainPhotoId = table.Column<int>(type: "int", nullable: true),
                    MaxNumber = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    WebSiteUrls = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircleEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CircleEvents_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CircleEvents_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CircleEvents_Photos_MainPhotoId",
                        column: x => x.MainPhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CircleEventParticipations",
                columns: table => new
                {
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    CircleEventId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Message = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircleEventParticipations", x => new { x.AppUserId, x.CircleEventId });
                    table.ForeignKey(
                        name: "FK_CircleEventParticipations_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CircleEventParticipations_CircleEvents_CircleEventId",
                        column: x => x.CircleEventId,
                        principalTable: "CircleEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_CircleEventId",
                table: "Photos",
                column: "CircleEventId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleEventParticipations_CircleEventId",
                table: "CircleEventParticipations",
                column: "CircleEventId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleEvents_AppUserId",
                table: "CircleEvents",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleEvents_CityId",
                table: "CircleEvents",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleEvents_MainPhotoId",
                table: "CircleEvents",
                column: "MainPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_CircleEvents_CircleEventId",
                table: "Photos",
                column: "CircleEventId",
                principalTable: "CircleEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_CircleEvents_CircleEventId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "CircleEventParticipations");

            migrationBuilder.DropTable(
                name: "CircleEvents");

            migrationBuilder.DropIndex(
                name: "IX_Photos_CircleEventId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "CircleEventId",
                table: "Photos");
        }
    }
}
