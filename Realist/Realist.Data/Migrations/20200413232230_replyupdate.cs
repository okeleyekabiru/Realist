using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Realist.Data.Migrations
{
    public partial class replyupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReplyId",
                table: "Replies",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Replies_ReplyId",
                table: "Replies",
                column: "ReplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Replies_ReplyId",
                table: "Replies",
                column: "ReplyId",
                principalTable: "Replies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Replies_ReplyId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_ReplyId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "ReplyId",
                table: "Replies");
        }
    }
}
