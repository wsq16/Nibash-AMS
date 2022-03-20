using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatManagement.Migrations
{
    public partial class AddTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommonFundDueDate",
                table: "CommonFunds");

            migrationBuilder.AddColumn<string>(
                name: "CommonFundYear",
                table: "CommonFunds",
                type: "nvarchar(25)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommonFundYear",
                table: "CommonFunds");

            migrationBuilder.AddColumn<DateTime>(
                name: "CommonFundDueDate",
                table: "CommonFunds",
                type: "DateTime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
