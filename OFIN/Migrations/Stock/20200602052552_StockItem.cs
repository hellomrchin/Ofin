using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OFIN.Migrations.Stock
{
    public partial class StockItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tStock",
                columns: table => new
                {
                    ItemCode = table.Column<string>(nullable: false),
                    ItemStock = table.Column<int>(nullable: false),
                    fLastUpdate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tStock", x => x.ItemCode);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tStock");
        }
    }
}
