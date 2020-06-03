using Microsoft.EntityFrameworkCore.Migrations;

namespace OFIN.Migrations.Supplier
{
    public partial class Supplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tSupplier",
                columns: table => new
                {
                    fSupplierCode = table.Column<string>(nullable: false),
                    fSupplierName = table.Column<string>(nullable: true),
                    fSupplierEmail = table.Column<string>(nullable: true),
                    fSupplierPhone = table.Column<string>(nullable: true),
                    fRemark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tSupplier", x => x.fSupplierCode);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tSupplier");
        }
    }
}
