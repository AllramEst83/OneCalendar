using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OneCalendar.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalenderGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IdsCollection = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalenderGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalenderTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TaskName = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TaskDescription = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    EventColor = table.Column<string>(nullable: true),
                    EventTextColor = table.Column<string>(nullable: true),
                    CalenderGroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalenderTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalenderTasks_CalenderGroups_CalenderGroupId",
                        column: x => x.CalenderGroupId,
                        principalTable: "CalenderGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EditedByUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateOfEdit = table.Column<DateTime>(nullable: false),
                    EditedByUserId = table.Column<string>(nullable: true),
                    CalenderTaskId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditedByUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EditedByUser_CalenderTasks_CalenderTaskId",
                        column: x => x.CalenderTaskId,
                        principalTable: "CalenderTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalenderTasks_CalenderGroupId",
                table: "CalenderTasks",
                column: "CalenderGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_EditedByUser_CalenderTaskId",
                table: "EditedByUser",
                column: "CalenderTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EditedByUser");

            migrationBuilder.DropTable(
                name: "CalenderTasks");

            migrationBuilder.DropTable(
                name: "CalenderGroups");
        }
    }
}
