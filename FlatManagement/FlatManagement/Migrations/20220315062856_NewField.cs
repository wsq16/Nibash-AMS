using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatManagement.Migrations
{
    public partial class NewField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "CommonFunds",
                newName: "CommonFundDueDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommonFundDueDate",
                table: "CommonFunds",
                newName: "DueDate");
        }
    }
}
