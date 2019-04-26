using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OneCalendar.Migrations
{
    public partial class addedCalenderGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalenderTasks_User_TaskCreatedByUserId",
                table: "CalenderTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_EditedByUser_User_EditedById",
                table: "EditedByUser");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_EditedByUser_EditedById",
                table: "EditedByUser");

            migrationBuilder.DropIndex(
                name: "IX_CalenderTasks_TaskCreatedByUserId",
                table: "CalenderTasks");

            migrationBuilder.DropColumn(
                name: "EditedById",
                table: "EditedByUser");

            migrationBuilder.AddColumn<int>(
                name: "EditedByUserId",
                table: "EditedByUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TaskCreatedByUserId",
                table: "CalenderTasks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditedByUserId",
                table: "EditedByUser");

            migrationBuilder.AddColumn<string>(
                name: "EditedById",
                table: "EditedByUser",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaskCreatedByUserId",
                table: "CalenderTasks",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EditedByUser_EditedById",
                table: "EditedByUser",
                column: "EditedById");

            migrationBuilder.CreateIndex(
                name: "IX_CalenderTasks_TaskCreatedByUserId",
                table: "CalenderTasks",
                column: "TaskCreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalenderTasks_User_TaskCreatedByUserId",
                table: "CalenderTasks",
                column: "TaskCreatedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EditedByUser_User_EditedById",
                table: "EditedByUser",
                column: "EditedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
