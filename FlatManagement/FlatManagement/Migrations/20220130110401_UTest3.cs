using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatManagement.Migrations
{
    public partial class UTest3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlowType = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    curr_ApprovedByRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Next_ApprovedByRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    PaidBy = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    PreparedBy = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    FlatNo = table.Column<string>(type: "nvarchar(35)", nullable: true),
                    FlatOwner = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    CurrentStatus = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TransactionCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Claim = table.Column<int>(type: "int", nullable: false),
                    SplitAmt = table.Column<int>(type: "int", nullable: false),
                    ReceiptFile = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    BillType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BillFor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillAmount = table.Column<double>(type: "float", nullable: false),
                    BillDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Processes");
        }
    }
}
