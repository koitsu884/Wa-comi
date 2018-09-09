using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class madeRadiusNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Radius",
                table: "Attractions",
                type: "int",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 5000);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Radius",
                table: "Attractions",
                maxLength: 5000,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 5000,
                oldNullable: true);
        }
    }
}
