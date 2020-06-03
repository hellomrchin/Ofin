using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OFIN.Migrations.Admin
{
    public partial class Admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tAdmin",
                columns: table => new
                {
                    fUsername = table.Column<string>(nullable: false),
                    fPassword = table.Column<string>(nullable: true),
                    fAccessToken = table.Column<string>(nullable: true),
                    fRegTime = table.Column<DateTime>(nullable: false),
                    fIsVerified = table.Column<string>(nullable: true),
                    fLastLogin = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tAdmin", x => x.fUsername);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tAdmin");
        }
    }
}
