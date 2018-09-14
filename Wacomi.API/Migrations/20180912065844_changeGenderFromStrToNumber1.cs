using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class changeGenderFromStrToNumber1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "MemberProfiles");

            migrationBuilder.AddColumn<string>(
                name: "GenderStr",
                table: "MemberProfiles",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenderStr",
                table: "MemberProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "MemberProfiles",
                nullable: true);
        }
    }
}
