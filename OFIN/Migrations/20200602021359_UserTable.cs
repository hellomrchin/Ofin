using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OFIN.Migrations
{
    public partial class UserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tUser",
                columns: table => new
                {
                    fUsername = table.Column<string>(nullable: false),
                    fPassword = table.Column<string>(nullable: true),
                    fAccessToken = table.Column<string>(nullable: true),
                    fRegTime = table.Column<DateTime>(nullable: false),
                    fIsVerified = table.Column<DateTime>(nullable: false),
                    fLastLogin = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tUser", x => x.fUsername);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tUser");
        }
    }
}
