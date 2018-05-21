using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addClanPropertySeek : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "ClanSeeks");

            migrationBuilder.AddColumn<int>(
                name: "PropertySeekId",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ClanSeeks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClanSeekCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClanSeekCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertySeekCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertySeekCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertySeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastActive = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainPhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                        name: "FK_PropertySeeks_AspNetUsers_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PropertySeekId",
                table: "Photos",
                column: "PropertySeekId");

            migrationBuilder.CreateIndex(
                name: "IX_ClanSeeks_CategoryId",
                table: "ClanSeeks",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySeeks_CategoryId",
                table: "PropertySeeks",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySeeks_OwnerUserId",
                table: "PropertySeeks",
                column: "OwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClanSeeks_ClanSeekCategories_CategoryId",
                table: "ClanSeeks",
                column: "CategoryId",
                principalTable: "ClanSeekCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_PropertySeeks_PropertySeekId",
                table: "Photos",
                column: "PropertySeekId",
                principalTable: "PropertySeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClanSeeks_ClanSeekCategories_CategoryId",
                table: "ClanSeeks");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_PropertySeeks_PropertySeekId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "ClanSeekCategories");

            migrationBuilder.DropTable(
                name: "PropertySeeks");

            migrationBuilder.DropTable(
                name: "PropertySeekCategories");

            migrationBuilder.DropIndex(
                name: "IX_Photos_PropertySeekId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_ClanSeeks_CategoryId",
                table: "ClanSeeks");

            migrationBuilder.DropColumn(
                name: "PropertySeekId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ClanSeeks");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ClanSeeks",
                nullable: true);
        }
    }
}
