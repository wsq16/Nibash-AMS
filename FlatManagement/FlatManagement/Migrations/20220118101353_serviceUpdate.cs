using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatManagement.Migrations
{
    public partial class serviceUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileUpload",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "FileUpload1",
                table: "Services",
                type: "nvarchar(56)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUpload2",
                table: "Services",
                type: "nvarchar(56)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUpload3",
                table: "Services",
                type: "nvarchar(56)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileUpload1",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "FileUpload2",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "FileUpload3",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "FileUpload",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
