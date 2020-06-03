using Microsoft.EntityFrameworkCore.Migrations;

namespace OFIN.Migrations.AdminDetail
{
    public partial class Admin2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tAdminDetail",
                columns: table => new
                {
                    fUsername = table.Column<string>(nullable: false),
                    fIcNumber = table.Column<string>(nullable: true),
                    fFirstName = table.Column<string>(nullable: true),
                    fLastName = table.Column<string>(nullable: true),
                    fGender = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tAdminDetail", x => x.fUsername);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tAdminDetail");
        }
    }
}
