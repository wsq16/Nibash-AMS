using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatManagement.Migrations
{
    public partial class UTest1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemStatus",
                table: "ApprovalLimits");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "ApprovalLimits",
                newName: "FlowTypes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FlowTypes",
                table: "ApprovalLimits",
                newName: "country");

            migrationBuilder.AddColumn<int>(
                name: "ItemStatus",
                table: "ApprovalLimits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
