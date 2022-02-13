using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatManagement.Migrations
{
    public partial class UpdateApprovalTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AmountLimit",
                table: "ApprovalLimits",
                newName: "AmountLimit_MIN");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountLimit_MAX",
                table: "ApprovalLimits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountLimit_MAX",
                table: "ApprovalLimits");

            migrationBuilder.RenameColumn(
                name: "AmountLimit_MIN",
                table: "ApprovalLimits",
                newName: "AmountLimit");
        }
    }
}
