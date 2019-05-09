using Microsoft.EntityFrameworkCore.Migrations;

namespace OneCalendar.Migrations
{
    public partial class AddedColorAndTextColorToTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "CalenderTasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextColor",
                table: "CalenderTasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "CalenderTasks");

            migrationBuilder.DropColumn(
                name: "TextColor",
                table: "CalenderTasks");
        }
    }
}
