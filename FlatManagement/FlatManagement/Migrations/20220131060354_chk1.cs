using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatManagement.Migrations
{
    public partial class chk1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FlowType",
                table: "Processes",
                newName: "Flow");

            migrationBuilder.AddColumn<int>(
                name: "FlowTypes",
                table: "Processes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlowTypes",
                table: "Processes");

            migrationBuilder.RenameColumn(
                name: "Flow",
                table: "Processes",
                newName: "FlowType");
        }
    }
}
