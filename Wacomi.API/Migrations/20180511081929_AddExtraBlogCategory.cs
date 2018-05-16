using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class AddExtraBlogCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category2",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category3",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Category",
                table: "Blogs",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Category2",
                table: "Blogs",
                column: "Category2");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Category3",
                table: "Blogs",
                column: "Category3");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Blogs_Category",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_Category2",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_Category3",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Category2",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Category3",
                table: "Blogs");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Blogs",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
