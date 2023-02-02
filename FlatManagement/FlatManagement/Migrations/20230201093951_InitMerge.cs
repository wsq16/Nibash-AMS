using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatManagement.Migrations
{
    public partial class InitMerge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Merged",
                table: "FlatConfigs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MergerCount",
                table: "FlatConfigs",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Merged",
                table: "FlatConfigs");

            migrationBuilder.DropColumn(
                name: "MergerCount",
                table: "FlatConfigs");
        }
    }
}
