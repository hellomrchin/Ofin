using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OFIN.Migrations.Balance
{
    public partial class MigrateBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tBalance",
                columns: table => new
                {
                    fUsername = table.Column<string>(nullable: false),
                    fBalance = table.Column<int>(nullable: false),
                    fLastTopUp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tBalance", x => x.fUsername);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tBalance");
        }
    }
}
