using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class changeDateFormatOfCircleEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromTime",
                table: "CircleEvents");

            migrationBuilder.DropColumn(
                name: "ToTime",
                table: "CircleEvents");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "CircleEvents",
                newName: "FromDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "CircleEvents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "CircleEvents");

            migrationBuilder.RenameColumn(
                name: "FromDate",
                table: "CircleEvents",
                newName: "Date");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "FromTime",
                table: "CircleEvents",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ToTime",
                table: "CircleEvents",
                nullable: true);
        }
    }
}
