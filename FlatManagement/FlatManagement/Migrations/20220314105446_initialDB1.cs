using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatManagement.Migrations
{
    public partial class initialDB1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AdvancePaymentDueDate",
                table: "AdvancedPaymentTbl",
                type: "DateTime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "AdvancedPaymentTbl",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvancePaymentDueDate",
                table: "AdvancedPaymentTbl");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "AdvancedPaymentTbl");
        }
    }
}
