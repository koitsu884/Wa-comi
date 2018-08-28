using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class someUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogFeedComments_Photos_PhotoId",
                table: "BlogFeedComments");

            migrationBuilder.DropIndex(
                name: "IX_BlogFeedComments_PhotoId",
                table: "BlogFeedComments");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "BlogFeedComments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRssRead",
                table: "Blogs",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRssRead",
                table: "Blogs",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "BlogFeedComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogFeedComments_PhotoId",
                table: "BlogFeedComments",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogFeedComments_Photos_PhotoId",
                table: "BlogFeedComments",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
