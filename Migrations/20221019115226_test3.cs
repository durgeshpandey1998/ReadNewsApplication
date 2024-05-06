using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsApp.Migrations
{
    public partial class test3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Author",
                table: "NewsContent",
                newName: "author");

            migrationBuilder.RenameColumn(
                name: "SourceName",
                table: "NewsContent",
                newName: "urlToImage");

            migrationBuilder.RenameColumn(
                name: "NewsHeading",
                table: "NewsContent",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "NewsDescription",
                table: "NewsContent",
                newName: "source");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "NewsContent",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "NewsContent",
                newName: "publishedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "author",
                table: "NewsContent",
                newName: "Author");

            migrationBuilder.RenameColumn(
                name: "urlToImage",
                table: "NewsContent",
                newName: "SourceName");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "NewsContent",
                newName: "NewsHeading");

            migrationBuilder.RenameColumn(
                name: "source",
                table: "NewsContent",
                newName: "NewsDescription");

            migrationBuilder.RenameColumn(
                name: "publishedAt",
                table: "NewsContent",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "NewsContent",
                newName: "Image");
        }
    }
}
