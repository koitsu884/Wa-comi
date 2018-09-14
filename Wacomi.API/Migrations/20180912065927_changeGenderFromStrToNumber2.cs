using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class changeGenderFromStrToNumber2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenderStr",
                table: "MemberProfiles");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "MemberProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "MemberProfiles");

            migrationBuilder.AddColumn<string>(
                name: "GenderStr",
                table: "MemberProfiles",
                nullable: true);
        }
    }
}
