using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addFewStringColumnsOnNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromUserName",
                table: "Notifications",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetRecordTitle",
                table: "Notifications",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromUserName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "TargetRecordTitle",
                table: "Notifications");
        }
    }
}
