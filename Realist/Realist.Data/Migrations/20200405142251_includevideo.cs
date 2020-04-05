using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Realist.Data.Migrations
{
    public partial class includevideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoPublicId",
                table: "Videos");

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "Videos",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PostId1",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Articles",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "News",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "Photos",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PostId1",
                table: "Photos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_PostId",
                table: "Videos",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_PostId1",
                table: "Videos",
                column: "PostId1");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PostId",
                table: "Photos",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PostId1",
                table: "Photos",
                column: "PostId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Posts_PostId",
                table: "Photos",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Posts_PostId1",
                table: "Photos",
                column: "PostId1",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Posts_PostId",
                table: "Videos",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Posts_PostId1",
                table: "Videos",
                column: "PostId1",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Posts_PostId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Posts_PostId1",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Posts_PostId",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Posts_PostId1",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_PostId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_PostId1",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_PostId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_PostId1",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Articles",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "News",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "VideoPublicId",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
