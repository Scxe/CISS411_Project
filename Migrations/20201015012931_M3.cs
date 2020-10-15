using Microsoft.EntityFrameworkCore.Migrations;

namespace CISS411_Project.Migrations
{
    public partial class M3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Swimmers_SwimmerId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_SwimmerId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "SwimmerId",
                table: "Sessions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SwimmerId",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SwimmerId",
                table: "Sessions",
                column: "SwimmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Swimmers_SwimmerId",
                table: "Sessions",
                column: "SwimmerId",
                principalTable: "Swimmers",
                principalColumn: "SwimmerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
