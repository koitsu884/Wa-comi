using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addDateRssReadOnBlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRssRead",
                table: "Blogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DateCreated",
                table: "Messages",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "IX_ClanSeeks_Created",
                table: "ClanSeeks",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_DateRssRead",
                table: "Blogs",
                column: "DateRssRead");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_DateCreated",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_ClanSeeks_Created",
                table: "ClanSeeks");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_DateRssRead",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "DateRssRead",
                table: "Blogs");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Blogs",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Blogs",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
