using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsApp.Migrations
{
    public partial class NewsContents2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsContent_Registers_UserId",
                table: "NewsContent");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NewsContent",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_NewsContent_UserId",
                table: "NewsContent",
                newName: "IX_NewsContent_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsContent_Registers_Id",
                table: "NewsContent",
                column: "Id",
                principalTable: "Registers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsContent_Registers_Id",
                table: "NewsContent");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "NewsContent",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsContent_Id",
                table: "NewsContent",
                newName: "IX_NewsContent_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsContent_Registers_UserId",
                table: "NewsContent",
                column: "UserId",
                principalTable: "Registers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
