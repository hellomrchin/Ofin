using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OFIN.Migrations.Payment
{
    public partial class PaymentCon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tPayment",
                columns: table => new
                {
                    fPaymentId = table.Column<string>(nullable: false),
                    fUsername = table.Column<string>(nullable: true),
                    fItemCode = table.Column<string>(nullable: true),
                    fQty = table.Column<int>(nullable: false),
                    fPaymentDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tPayment", x => x.fPaymentId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tPayment");
        }
    }
}
