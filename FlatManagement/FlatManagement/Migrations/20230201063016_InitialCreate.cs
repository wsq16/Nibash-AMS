using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatManagement.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvancedPaymentTbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonType = table.Column<string>(type: "varchar(50)", nullable: false),
                    PersonEmail = table.Column<string>(type: "varchar(50)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    CollectionDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    CollectionBy = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Month = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    AdvancePaymentDueDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    PaymentStatus = table.Column<string>(type: "varchar(50)", nullable: false),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedPaymentTbl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Agendas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgendaName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AgendaDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    LocationStr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AgendaDetails = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Attachment = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agendas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalLimits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(65)", nullable: false),
                    Flow = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountLimit_MIN = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountLimit_MAX = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    PreparedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlowTypes = table.Column<int>(type: "int", nullable: false),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ETIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Per_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pre_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flat_No = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tenant = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    FlatOwner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillType = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    BillDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    BillFor = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    FlatNo = table.Column<string>(type: "varchar(250)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ReceivedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReceivedAmountNotes = table.Column<string>(type: "varchar(250)", nullable: true),
                    Remarks = table.Column<string>(type: "varchar(250)", nullable: true),
                    PreparedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    BillStatus = table.Column<string>(type: "varchar(25)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DemoVal = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Committees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Roll = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Committees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommonFunds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FundType = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    FlatNo = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    FlatOwner = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    CollectionDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    CommonFundDueDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    CollectionBy = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Month = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonFunds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApartCodeName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LogoUri = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    License = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractName = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    BillType = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    BillFrequency = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    EmpNID = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Mobile = table.Column<string>(type: "varchar(25)", nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false),
                    JoiningDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EntryBy = table.Column<string>(type: "varchar(256)", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    PicLoc = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnumValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "varchar(20)", nullable: false),
                    EnumText = table.Column<string>(type: "varchar(25)", nullable: false),
                    EnumValueType = table.Column<int>(type: "int", nullable: false),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Faqs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaqTitle = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    FaqDescription = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    EntryBy = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    Date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faqs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlatConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    slId = table.Column<int>(type: "int", nullable: false),
                    isWing = table.Column<bool>(type: "bit", nullable: false),
                    Wing = table.Column<int>(type: "int", nullable: true),
                    TotalWing = table.Column<int>(type: "int", nullable: true),
                    GroundFloorStart = table.Column<int>(type: "int", nullable: true),
                    WingPerFloor = table.Column<int>(type: "int", nullable: true),
                    FlatPerWing = table.Column<int>(type: "int", nullable: true),
                    Floor = table.Column<int>(type: "int", nullable: true),
                    FlatNo = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    WingName = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    TotalFlat = table.Column<int>(type: "int", nullable: true),
                    FlatPerFloor = table.Column<int>(type: "int", nullable: true),
                    FlatStartFrom = table.Column<int>(type: "int", nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: true),
                    Delimeter = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlatConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Maintenances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    maintenance_type = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    contract = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    entry_by = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    entry_date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    DOB = table.Column<DateTime>(type: "DateTime", nullable: false),
                    NID = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    ETIN = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    PassportNo = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    Per_Address = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    pre_Address = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Flat_No = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    UserRole = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Tenant = table.Column<bool>(type: "bit", nullable: false),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimRefId = table.Column<int>(type: "int", nullable: false),
                    Flow = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    curr_ApprovedByRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Next_ApprovedByRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    PaidBy = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    PreparedBy = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    FlatNo = table.Column<string>(type: "nvarchar(35)", nullable: true),
                    FlatOwner = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: false),
                    CurrentStatus = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TransactionCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Claim = table.Column<int>(type: "int", nullable: false),
                    SplitAmt = table.Column<double>(type: "float", nullable: false),
                    ReceiptFile = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    BillType = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    BillDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BillFor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillAmount = table.Column<double>(type: "float", nullable: false),
                    BillDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlowTypes = table.Column<int>(type: "int", nullable: false),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Receives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiveType = table.Column<string>(type: "varchar(50)", nullable: false),
                    ReceiveFrom = table.Column<string>(type: "varchar(50)", nullable: false),
                    ReceiveFor = table.Column<string>(type: "varchar(50)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    CollectionDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    CollectionBy = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Month = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receives", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resolutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgendaName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PointNo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Resolution = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ResolutionClosingNote = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ResponsibilityFlatOwner = table.Column<string>(type: "nvarchar(45)", nullable: true),
                    ResponsibilityEmployee = table.Column<string>(type: "nvarchar(45)", nullable: true),
                    ResponsibilityEmployeeName = table.Column<string>(type: "nvarchar(45)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    DueDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    startTime = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    endTime = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Attachment = table.Column<string>(type: "nvarchar(55)", nullable: true),
                    msg_SMS = table.Column<bool>(type: "bit", nullable: false),
                    msg_EMAIL = table.Column<bool>(type: "bit", nullable: false),
                    msg_BOTH = table.Column<bool>(type: "bit", nullable: false),
                    to_Committee = table.Column<bool>(type: "bit", nullable: false),
                    to_Treasurer = table.Column<bool>(type: "bit", nullable: false),
                    to_FlatOwner = table.Column<bool>(type: "bit", nullable: false),
                    to_All = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resolutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    BillType = table.Column<string>(type: "nvarchar(55)", nullable: false),
                    FileUpload1 = table.Column<string>(type: "nvarchar(56)", nullable: true),
                    FileUpload2 = table.Column<string>(type: "nvarchar(56)", nullable: true),
                    FileUpload3 = table.Column<string>(type: "nvarchar(56)", nullable: true),
                    PrepairedBy = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(56)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    PrepairedBy = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    Start_Date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    End_Date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Per_Address = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Pre_Address = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    NID = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    ETIN = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    ApartCodeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ownerId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    DOB = table.Column<DateTime>(type: "DateTime", nullable: false),
                    NID = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    ETIN = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    PassportNo = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    Per_Address = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    pre_Address = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Purpose = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    PaidBy = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    PreparedBy = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    FlatNo = table.Column<string>(type: "nvarchar(35)", nullable: true),
                    FlatOwner = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TransactionStep = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Claim = table.Column<int>(type: "int", nullable: false),
                    ReceiptFile = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TransactionCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextStatus = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    BillType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BillFor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillAmount = table.Column<double>(type: "float", nullable: false),
                    BillDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvancedPaymentTbl");

            migrationBuilder.DropTable(
                name: "Agendas");

            migrationBuilder.DropTable(
                name: "ApprovalLimits");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Committees");

            migrationBuilder.DropTable(
                name: "CommonFunds");

            migrationBuilder.DropTable(
                name: "CompanyInfo");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "DelegatePersonHistorys");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "EnumValues");

            migrationBuilder.DropTable(
                name: "Faqs");

            migrationBuilder.DropTable(
                name: "FlatConfigs");

            migrationBuilder.DropTable(
                name: "Maintenances");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "Processes");

            migrationBuilder.DropTable(
                name: "Receives");

            migrationBuilder.DropTable(
                name: "Resolutions");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
