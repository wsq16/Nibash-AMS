using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatManagement.Migrations
{
    public partial class processTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeleteFlag",
                table: "Processes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Processes",
                type: "nvarchar(256)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteFlag",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Processes");
        }
    }
}
