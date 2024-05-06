using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsApp.Migrations
{
    public partial class NewsContent1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "NewsContent",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "dateTime",
                table: "NewsContent",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_NewsContent_UserId",
                table: "NewsContent",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsContent_Registers_UserId",
                table: "NewsContent",
                column: "UserId",
                principalTable: "Registers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsContent_Registers_UserId",
                table: "NewsContent");

            migrationBuilder.DropIndex(
                name: "IX_NewsContent_UserId",
                table: "NewsContent");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NewsContent");

            migrationBuilder.DropColumn(
                name: "dateTime",
                table: "NewsContent");
        }
    }
}
