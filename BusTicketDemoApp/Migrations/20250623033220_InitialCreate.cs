using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusTicketDemoApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusLocations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusLocations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "BusSchedules",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    BusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BusVehicleNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FromLocation = table.Column<int>(type: "int", nullable: false),
                    ToLocation = table.Column<int>(type: "int", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusSchedules", x => x.ScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VendorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorId);
                });

            migrationBuilder.CreateTable(
                name: "BusBookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusBookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_BusBookings_BusSchedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "BusSchedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusBookingPassengers",
                columns: table => new
                {
                    PassengerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    PassengerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SeatNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusBookingPassengers", x => x.PassengerId);
                    table.ForeignKey(
                        name: "FK_BusBookingPassengers_BusBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "BusBookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BusLocations",
                columns: new[] { "LocationId", "Code", "LocationName" },
                values: new object[,]
                {
                    { 1, "CDMX", "Ciudad de México" },
                    { 2, "GDL", "Guadalajara" },
                    { 3, "MTY", "Monterrey" },
                    { 4, "PUE", "Puebla" },
                    { 5, "CUN", "Cancún" }
                });

            migrationBuilder.InsertData(
                table: "BusSchedules",
                columns: new[] { "ScheduleId", "ArrivalTime", "BusName", "BusVehicleNo", "DepartureTime", "FromLocation", "Price", "ScheduleDate", "ToLocation", "TotalSeats", "VendorId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 23, 14, 0, 0, 0, DateTimeKind.Unspecified), "Express CDMX-GDL", "ADN-001", new DateTime(2025, 6, 23, 8, 0, 0, 0, DateTimeKind.Unspecified), 1, 450.00m, new DateTime(2025, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 42, 1 },
                    { 2, new DateTime(2025, 6, 23, 20, 0, 0, 0, DateTimeKind.Unspecified), "Ejecutivo CDMX-MTY", "ADN-002", new DateTime(2025, 6, 23, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, 650.00m, new DateTime(2025, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 38, 1 },
                    { 3, new DateTime(2025, 6, 24, 16, 0, 0, 0, DateTimeKind.Unspecified), "Primera Plus GDL-CUN", "PP-101", new DateTime(2025, 6, 23, 22, 0, 0, 0, DateTimeKind.Unspecified), 2, 980.00m, new DateTime(2025, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 40, 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedDate", "EmailId", "FullName", "Password", "ProjectName", "RefreshToken", "RefreshTokenExpiryTime", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@busticket.com", "Administrador del Sistema", "admin123", "BusTicketDemo", null, null, "Admin", "admin" },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "customer1@email.com", "Juan Pérez", "customer123", "BusTicketDemo", null, null, "Customer", "customer1" },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vendor1@email.com", "María González", "vendor123", "BusTicketDemo", null, null, "Vendor", "vendor1" }
                });

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "VendorId", "CreatedDate", "Email", "Phone", "VendorName" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "info@autobusdelnorte.com", "+52-55-1234-5678", "Autobuses del Norte" },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "contacto@primeraplus.com", "+52-33-9876-5432", "Primera Plus" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusBookingPassengers_BookingId_SeatNo",
                table: "BusBookingPassengers",
                columns: new[] { "BookingId", "SeatNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusBookings_ScheduleId",
                table: "BusBookings",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_BusLocations_Code",
                table: "BusLocations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusSchedules_FromLocation_ToLocation_ScheduleDate",
                table: "BusSchedules",
                columns: new[] { "FromLocation", "ToLocation", "ScheduleDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailId",
                table: "Users",
                column: "EmailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusBookingPassengers");

            migrationBuilder.DropTable(
                name: "BusLocations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "BusBookings");

            migrationBuilder.DropTable(
                name: "BusSchedules");
        }
    }
}
