using Microsoft.EntityFrameworkCore.Migrations;

namespace OFIN.Migrations.ActionResponseMigrations
{
    public partial class actionResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tActionResponses",
                columns: table => new
                {
                    fErrorCode = table.Column<string>(nullable: false),
                    fErrorDesc = table.Column<string>(nullable: true),
                    fErrorMsg = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tActionResponses", x => x.fErrorCode);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tActionResponses");
        }
    }
}
