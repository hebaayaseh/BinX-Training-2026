using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardioTrack.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Appointments",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId_Status",
                table: "Appointments",
                columns: new[] { "DoctorId", "Status" });

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Vital-Sign-Alerts_PatientId",
                table: "Vital-Sign-Alerts",
                column: "PatientId");

            migrationBuilder.DropIndex(
                name: "IX_Vital-Sign-Alerts_PatientId_IsResolved",
                table: "Vital-Sign-Alerts");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId_Status",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Appointments",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
