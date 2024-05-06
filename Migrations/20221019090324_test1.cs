using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsApp.Migrations
{
    public partial class test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "NewsContent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "NewsContent",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "NewsContent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewsHeading",
                table: "NewsContent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceName",
                table: "NewsContent",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "NewsContent");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "NewsContent");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "NewsContent");

            migrationBuilder.DropColumn(
                name: "NewsHeading",
                table: "NewsContent");

            migrationBuilder.DropColumn(
                name: "SourceName",
                table: "NewsContent");
        }
    }
}
