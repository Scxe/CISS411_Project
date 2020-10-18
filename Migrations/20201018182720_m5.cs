using Microsoft.EntityFrameworkCore.Migrations;

namespace CISS411_Project.Migrations
{
    public partial class m5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Swimmers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Swimmers_UserId",
                table: "Swimmers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Swimmers_AspNetUsers_UserId",
                table: "Swimmers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Swimmers_AspNetUsers_UserId",
                table: "Swimmers");

            migrationBuilder.DropIndex(
                name: "IX_Swimmers_UserId",
                table: "Swimmers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Swimmers");
        }
    }
}
