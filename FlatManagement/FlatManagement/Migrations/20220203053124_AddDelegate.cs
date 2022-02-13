using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatManagement.Migrations
{
    public partial class AddDelegate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DelegatePersonHistorys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DelegateType = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    AssignByUsername = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    AssignToUsername = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    DelegetePersonId = table.Column<int>(type: "int", nullable: false),
                    AssignResponsiblityDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegatePersonHistorys", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DelegatePersonHistorys");
        }
    }
}
