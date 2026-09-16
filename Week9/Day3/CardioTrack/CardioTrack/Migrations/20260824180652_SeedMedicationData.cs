using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardioTrack.Migrations
{
    /// <inheritdoc />
    public partial class SeedMedicationData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Medications",
                columns: new[] { "Id", "Dosage", "DrugName", "EndDate", "Frequency", "IsActive", "PatientId", "PrescribedByDoctorId", "StartDate" },
                values: new object[] { 3, "10mg", "Amlodipine", new DateTime(2027, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Once Daily", true, 3, 5, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Medications",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
