using Microsoft.EntityFrameworkCore.Migrations;

namespace CISS411_Project.Migrations
{
    public partial class M6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProgressReport",
                table: "Sessions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProgressReport",
                table: "Sessions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
