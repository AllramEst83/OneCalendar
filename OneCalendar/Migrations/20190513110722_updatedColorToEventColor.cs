using Microsoft.EntityFrameworkCore.Migrations;

namespace OneCalendar.Migrations
{
    public partial class updatedColorToEventColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Color",
                table: "CalenderTasks",
                newName: "EventColor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EventColor",
                table: "CalenderTasks",
                newName: "Color");
        }
    }
}
