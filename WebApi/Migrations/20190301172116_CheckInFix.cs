using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class CheckInFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckIn_Patients_PatientId",
                table: "CheckIn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckIn",
                table: "CheckIn");

            migrationBuilder.RenameTable(
                name: "CheckIn",
                newName: "CheckIns");

            migrationBuilder.RenameIndex(
                name: "IX_CheckIn_PatientId",
                table: "CheckIns",
                newName: "IX_CheckIns_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckIns",
                table: "CheckIns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckIns_Patients_PatientId",
                table: "CheckIns",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckIns_Patients_PatientId",
                table: "CheckIns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckIns",
                table: "CheckIns");

            migrationBuilder.RenameTable(
                name: "CheckIns",
                newName: "CheckIn");

            migrationBuilder.RenameIndex(
                name: "IX_CheckIns_PatientId",
                table: "CheckIn",
                newName: "IX_CheckIn_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckIn",
                table: "CheckIn",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckIn_Patients_PatientId",
                table: "CheckIn",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
