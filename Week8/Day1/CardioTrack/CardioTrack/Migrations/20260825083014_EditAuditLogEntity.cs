using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardioTrack.Migrations
{
    /// <inheritdoc />
    public partial class EditAuditLogEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Medications",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "NewValue",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "OldValue",
                table: "AuditLog");

            }
    }
}
