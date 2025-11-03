using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CongestionTaxCalculator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MaximumTaxPerDayAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumTaxPerDayCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    SingleChargeDurationMinutes = table.Column<int>(type: "int", nullable: false),
                    IsHolidayTaxExempt = table.Column<bool>(type: "bit", nullable: false),
                    IsDayBeforeHolidayTaxExempt = table.Column<bool>(type: "bit", nullable: false),
                    IsWeekendTaxExempt = table.Column<bool>(type: "bit", nullable: false),
                    IsJulyTaxExempt = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holidays_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    AmountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxRules_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxExemptVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxExemptVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxExemptVehicles_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxExemptVehicles_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TollPassages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassageTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppliedTax_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AppliedTax_Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TollPassages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TollPassages_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TollPassages_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "IsDayBeforeHolidayTaxExempt", "IsHolidayTaxExempt", "IsJulyTaxExempt", "IsWeekendTaxExempt", "ModifiedBy", "ModifiedOn", "Name", "SingleChargeDurationMinutes", "MaximumTaxPerDayAmount", "MaximumTaxPerDayCurrency" },
                values: new object[] { 1, null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), true, true, true, true, null, null, "Gothenburg", 60, 60m, "SEK" });

            migrationBuilder.InsertData(
                table: "Holidays",
                columns: new[] { "Id", "CityId", "CreatedBy", "CreatedOn", "Date", "Description", "ModifiedBy", "ModifiedOn" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "New Year’s Day", null, null },
                    { 2, null, null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Christmas Day", null, null }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn", "RegistrationNumber", "VehicleType" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), null, null, "ABC123", 0 },
                    { new Guid("00000000-0000-0000-0000-000000000002"), null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), null, null, "DEF456", 3 },
                    { new Guid("00000000-0000-0000-0000-000000000003"), null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), null, null, "GHI789", 4 },
                    { new Guid("00000000-0000-0000-0000-000000000004"), null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), null, null, "JKL012", 1 },
                    { new Guid("00000000-0000-0000-0000-000000000005"), null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), null, null, "MNO345", 2 },
                    { new Guid("00000000-0000-0000-0000-000000000006"), null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), null, null, "PQR678", 5 },
                    { new Guid("00000000-0000-0000-0000-000000000007"), null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), null, null, "STU901", 6 }
                });

            migrationBuilder.InsertData(
                table: "TaxExemptVehicles",
                columns: new[] { "Id", "CityId", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn", "VehicleId" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), null, null, new Guid("00000000-0000-0000-0000-000000000002") },
                    { 2, 1, null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), null, null, new Guid("00000000-0000-0000-0000-000000000003") },
                    { 3, 1, null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), null, null, new Guid("00000000-0000-0000-0000-000000000004") },
                    { 4, 1, null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), null, null, new Guid("00000000-0000-0000-0000-000000000005") },
                    { 5, 1, null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), null, null, new Guid("00000000-0000-0000-0000-000000000006") },
                    { 6, 1, null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), null, null, new Guid("00000000-0000-0000-0000-000000000007") }
                });

            migrationBuilder.InsertData(
                table: "TaxRules",
                columns: new[] { "Id", "CityId", "CreatedBy", "CreatedOn", "EndTime", "ModifiedBy", "ModifiedOn", "StartTime", "AmountValue", "AmountCurrency" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), new TimeSpan(0, 6, 30, 0, 0), null, null, new TimeSpan(0, 6, 0, 0, 0), 8m, "SEK" },
                    { 2, 1, null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), new TimeSpan(0, 7, 0, 0, 0), null, null, new TimeSpan(0, 6, 30, 0, 0), 13m, "SEK" },
                    { 3, 1, null, new DateTime(2025, 11, 2, 12, 32, 14, 0, DateTimeKind.Utc), new TimeSpan(0, 8, 0, 0, 0), null, null, new TimeSpan(0, 7, 0, 0, 0), 18m, "SEK" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_CityId",
                table: "Holidays",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxExemptVehicles_CityId",
                table: "TaxExemptVehicles",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxExemptVehicles_VehicleId",
                table: "TaxExemptVehicles",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRules_CityId",
                table: "TaxRules",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TollPassages_CityId_PassageTime",
                table: "TollPassages",
                columns: new[] { "CityId", "PassageTime" });

            migrationBuilder.CreateIndex(
                name: "IX_TollPassages_PassageTime",
                table: "TollPassages",
                column: "PassageTime");

            migrationBuilder.CreateIndex(
                name: "IX_TollPassages_VehicleId",
                table: "TollPassages",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "TaxExemptVehicles");

            migrationBuilder.DropTable(
                name: "TaxRules");

            migrationBuilder.DropTable(
                name: "TollPassages");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
