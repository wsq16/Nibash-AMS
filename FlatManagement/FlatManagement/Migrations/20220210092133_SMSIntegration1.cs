using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatManagement.Migrations
{
    public partial class SMSIntegration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "msg_BOTH",
                table: "Resolutions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "msg_EMAIL",
                table: "Resolutions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "msg_SMS",
                table: "Resolutions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "to_All",
                table: "Resolutions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "to_Committee",
                table: "Resolutions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "to_FlatOwner",
                table: "Resolutions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "to_Treasurer",
                table: "Resolutions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "msg_BOTH",
                table: "Resolutions");

            migrationBuilder.DropColumn(
                name: "msg_EMAIL",
                table: "Resolutions");

            migrationBuilder.DropColumn(
                name: "msg_SMS",
                table: "Resolutions");

            migrationBuilder.DropColumn(
                name: "to_All",
                table: "Resolutions");

            migrationBuilder.DropColumn(
                name: "to_Committee",
                table: "Resolutions");

            migrationBuilder.DropColumn(
                name: "to_FlatOwner",
                table: "Resolutions");

            migrationBuilder.DropColumn(
                name: "to_Treasurer",
                table: "Resolutions");
        }
    }
}
