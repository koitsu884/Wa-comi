using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addPhotoAndMessageOnNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PhotoId",
                table: "Notifications",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Photos_PhotoId",
                table: "Notifications",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Photos_PhotoId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_PhotoId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Notifications");
        }
    }
}
