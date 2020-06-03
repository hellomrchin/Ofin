using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OFIN.Migrations.UserLog
{
    public partial class MigrateLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tUserLogs",
                columns: table => new
                {
                    fLogId = table.Column<string>(nullable: false),
                    fUsername = table.Column<string>(nullable: true),
                    fAction = table.Column<string>(nullable: true),
                    fActionDesc = table.Column<string>(nullable: true),
                    fDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tUserLogs", x => x.fLogId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tUserLogs");
        }
    }
}
