using Microsoft.EntityFrameworkCore.Migrations;

namespace OFIN.Migrations.Item
{
    public partial class MigrationItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tMerchant",
                columns: table => new
                {
                    fItemCode = table.Column<string>(nullable: false),
                    fItemDesc = table.Column<string>(nullable: true),
                    fItemPrice = table.Column<string>(nullable: true),
                    fItemBought = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tMerchant", x => x.fItemCode);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tMerchant");
        }
    }
}
