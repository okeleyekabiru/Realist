using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Realist.Data.Migrations
{
    public partial class AddDeviceInfoAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Replies",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserInfoId",
                table: "Replies",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserInfoId",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserInfoId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Replies_UserId",
                table: "Replies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_UserInfoId",
                table: "Replies",
                column: "UserInfoId",
                unique: true,
                filter: "[UserInfoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserInfoId",
                table: "Posts",
                column: "UserInfoId",
                unique: true,
                filter: "[UserInfoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserInfoId",
                table: "Comments",
                column: "UserInfoId",
                unique: true,
                filter: "[UserInfoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_UserInfo_UserInfoId",
                table: "Comments",
                column: "UserInfoId",
                principalTable: "UserInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_UserInfo_UserInfoId",
                table: "Posts",
                column: "UserInfoId",
                principalTable: "UserInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_AspNetUsers_UserId",
                table: "Replies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_UserInfo_UserInfoId",
                table: "Replies",
                column: "UserInfoId",
                principalTable: "UserInfo",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_UserInfo_UserInfoId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_UserInfo_UserInfoId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_AspNetUsers_UserId",
                table: "Replies");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_UserInfo_UserInfoId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_UserId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_UserInfoId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserInfoId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserInfoId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "Comments");
        }
    }
}
