using Microsoft.EntityFrameworkCore.Migrations;

namespace OFIN.Migrations.UserDetail
{
    public partial class UserDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tUserDetail",
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
                    table.PrimaryKey("PK_tUserDetail", x => x.fUsername);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tUserDetail");
        }
    }
}
