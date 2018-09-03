using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class addPhotosOnAttractionReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttractionReviewId",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MainPhotoId",
                table: "AttractionReviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AttractionReviewId",
                table: "Photos",
                column: "AttractionReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionReviews_MainPhotoId",
                table: "AttractionReviews",
                column: "MainPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttractionReviews_Photos_MainPhotoId",
                table: "AttractionReviews",
                column: "MainPhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_AttractionReviews_AttractionReviewId",
                table: "Photos",
                column: "AttractionReviewId",
                principalTable: "AttractionReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttractionReviews_Photos_MainPhotoId",
                table: "AttractionReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_AttractionReviews_AttractionReviewId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_AttractionReviewId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_AttractionReviews_MainPhotoId",
                table: "AttractionReviews");

            migrationBuilder.DropColumn(
                name: "AttractionReviewId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "MainPhotoId",
                table: "AttractionReviews");
        }
    }
}
