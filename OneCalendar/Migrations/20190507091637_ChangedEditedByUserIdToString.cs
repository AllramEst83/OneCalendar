using Microsoft.EntityFrameworkCore.Migrations;

namespace OneCalendar.Migrations
{
    public partial class ChangedEditedByUserIdToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EditedByUserId",
                table: "EditedByUser",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EditedByUserId",
                table: "EditedByUser",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
