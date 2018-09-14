using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addPropertyInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_PropertySeeks_PropertySeekId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "PropertySeeks");

            migrationBuilder.DropIndex(
                name: "IX_Photos_PropertySeekId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PropertySeekId",
                table: "Photos");

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "PropertySeekCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateAvailable = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    HasChild = table.Column<bool>(type: "bit", nullable: false),
                    HasPet = table.Column<bool>(type: "bit", nullable: false),
                    Internet = table.Column<int>(type: "int", maxLength: 3, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastActive = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Latitude = table.Column<double>(type: "double", nullable: true),
                    Longitude = table.Column<double>(type: "double", nullable: true),
                    MainPhotoId = table.Column<int>(type: "int", nullable: true),
                    MaxTerm = table.Column<int>(type: "int", nullable: false),
                    MinTerm = table.Column<int>(type: "int", nullable: false),
                    Rent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Properties_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Properties_Photos_MainPhotoId",
                        column: x => x.MainPhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertySeekCategories_PropertyId",
                table: "PropertySeekCategories",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PropertyId",
                table: "Photos",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_AppUserId",
                table: "Properties",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_CityId",
                table: "Properties",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_MainPhotoId",
                table: "Properties",
                column: "MainPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Properties_PropertyId",
                table: "Photos",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertySeekCategories_Properties_PropertyId",
                table: "PropertySeekCategories",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Properties_PropertyId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertySeekCategories_Properties_PropertyId",
                table: "PropertySeekCategories");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_PropertySeekCategories_PropertyId",
                table: "PropertySeekCategories");

            migrationBuilder.DropIndex(
                name: "IX_Photos_PropertyId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "PropertySeekCategories");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Photos");

            migrationBuilder.AddColumn<int>(
                name: "PropertySeekId",
                table: "Photos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PropertySeeks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LastActive = table.Column<DateTime>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    MainPhotoId = table.Column<int>(nullable: true),
                    OwnerAppUserId = table.Column<string>(nullable: false),
                    OwnerAppUserId1 = table.Column<int>(nullable: true),
                    WebsiteUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertySeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertySeeks_PropertySeekCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "PropertySeekCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertySeeks_Cities_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertySeeks_Photos_MainPhotoId",
                        column: x => x.MainPhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertySeeks_AppUsers_OwnerAppUserId1",
                        column: x => x.OwnerAppUserId1,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PropertySeekId",
                table: "Photos",
                column: "PropertySeekId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySeeks_CategoryId",
                table: "PropertySeeks",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySeeks_LocationId",
                table: "PropertySeeks",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySeeks_MainPhotoId",
                table: "PropertySeeks",
                column: "MainPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySeeks_OwnerAppUserId1",
                table: "PropertySeeks",
                column: "OwnerAppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_PropertySeeks_PropertySeekId",
                table: "Photos",
                column: "PropertySeekId",
                principalTable: "PropertySeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
