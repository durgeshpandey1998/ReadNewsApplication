using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsApp.Migrations
{
    public partial class Payment1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Registers_RegisterUserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_RegisterUserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RegisterUserId",
                table: "Payments");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Registers_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "Registers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Registers_UserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserId",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "RegisterUserId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RegisterUserId",
                table: "Payments",
                column: "RegisterUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Registers_RegisterUserId",
                table: "Payments",
                column: "RegisterUserId",
                principalTable: "Registers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
