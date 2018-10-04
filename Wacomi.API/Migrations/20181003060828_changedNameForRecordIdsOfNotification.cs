using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class changedNameForRecordIdsOfNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalRecordId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RecordIdsStr",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "RelatingRecordIdsStr",
                table: "Notifications",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelatingRecordIdsStr",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "AdditionalRecordId",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecordIdsStr",
                table: "Notifications",
                nullable: true);
        }
    }
}
