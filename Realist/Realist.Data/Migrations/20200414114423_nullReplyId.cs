using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Realist.Data.Migrations
{
    public partial class nullReplyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Replies_ReplyId",
                table: "Replies");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReplyId",
                table: "Replies",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Replies_ReplyId",
                table: "Replies",
                column: "ReplyId",
                principalTable: "Replies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Replies_ReplyId",
                table: "Replies");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReplyId",
                table: "Replies",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Replies_ReplyId",
                table: "Replies",
                column: "ReplyId",
                principalTable: "Replies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
