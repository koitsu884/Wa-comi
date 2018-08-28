using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addIndexOnNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RecordType",
                table: "Notifications",
                type: "varchar(127)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AppUserId",
                table: "Notifications",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RecordId",
                table: "Notifications",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RecordType",
                table: "Notifications",
                column: "RecordType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_AppUserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_RecordId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_RecordType",
                table: "Notifications");

            migrationBuilder.AlterColumn<string>(
                name: "RecordType",
                table: "Notifications",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(127)",
                oldNullable: true);
        }
    }
}
