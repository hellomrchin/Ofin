using Microsoft.EntityFrameworkCore.Migrations;

namespace OFIN.Migrations.Log
{
    public partial class logContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tLogs",
                columns: table => new
                {
                    LogId = table.Column<string>(nullable: false),
                    LogGroup = table.Column<string>(nullable: true),
                    LogMsg = table.Column<string>(nullable: true),
                    ErrorCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tLogs", x => x.LogId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tLogs");
        }
    }
}
