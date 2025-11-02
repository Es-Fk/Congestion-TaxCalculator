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
                name: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
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
                        name: "FK_TollPassages_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "IsDayBeforeHolidayTaxExempt", "IsHolidayTaxExempt", "IsJulyTaxExempt", "IsWeekendTaxExempt", "ModifiedBy", "ModifiedOn", "Name", "SingleChargeDurationMinutes" },
                values: new object[] { 1, null, new DateTime(2025, 11, 2, 12, 32, 14, 130, DateTimeKind.Utc).AddTicks(6076), true, true, true, true, null, null, "Gothenburg", 60 });

            migrationBuilder.InsertData(
                table: "Holidays",
                columns: new[] { "Id", "CityId", "CreatedBy", "CreatedOn", "Date", "Description", "ModifiedBy", "ModifiedOn" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2025, 11, 2, 12, 32, 14, 130, DateTimeKind.Utc).AddTicks(9814), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "New Year’s Day", null, null },
                    { 2, null, null, new DateTime(2025, 11, 2, 12, 32, 14, 130, DateTimeKind.Utc).AddTicks(9817), new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Christmas Day", null, null }
                });

            migrationBuilder.InsertData(
                table: "TaxExemptVehicles",
                columns: new[] { "Id", "CityId", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn", "VehicleType" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2025, 11, 2, 12, 32, 14, 131, DateTimeKind.Utc).AddTicks(1881), null, null, 4 },
                    { 2, null, null, new DateTime(2025, 11, 2, 12, 32, 14, 131, DateTimeKind.Utc).AddTicks(1882), null, null, 2 },
                    { 3, null, null, new DateTime(2025, 11, 2, 12, 32, 14, 131, DateTimeKind.Utc).AddTicks(1883), null, null, 1 },
                    { 4, null, null, new DateTime(2025, 11, 2, 12, 32, 14, 131, DateTimeKind.Utc).AddTicks(1884), null, null, 5 },
                    { 5, null, null, new DateTime(2025, 11, 2, 12, 32, 14, 131, DateTimeKind.Utc).AddTicks(1885), null, null, 3 },
                    { 6, null, null, new DateTime(2025, 11, 2, 12, 32, 14, 131, DateTimeKind.Utc).AddTicks(1885), null, null, 6 }
                });

            migrationBuilder.InsertData(
                table: "TaxRules",
                columns: new[] { "Id", "CityId", "CreatedBy", "CreatedOn", "EndTime", "ModifiedBy", "ModifiedOn", "StartTime" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2025, 11, 2, 12, 32, 14, 135, DateTimeKind.Utc).AddTicks(2881), new TimeSpan(0, 6, 30, 0, 0), null, null, new TimeSpan(0, 6, 0, 0, 0) },
                    { 2, null, null, new DateTime(2025, 11, 2, 12, 32, 14, 135, DateTimeKind.Utc).AddTicks(2884), new TimeSpan(0, 7, 0, 0, 0), null, null, new TimeSpan(0, 6, 30, 0, 0) },
                    { 3, null, null, new DateTime(2025, 11, 2, 12, 32, 14, 135, DateTimeKind.Utc).AddTicks(2886), new TimeSpan(0, 8, 0, 0, 0), null, null, new TimeSpan(0, 7, 0, 0, 0) }
                });

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
                name: "TollPassages");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Holidays",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Holidays",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TaxExemptVehicles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TaxExemptVehicles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TaxExemptVehicles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TaxExemptVehicles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TaxExemptVehicles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TaxExemptVehicles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TaxRules",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TaxRules",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TaxRules",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
