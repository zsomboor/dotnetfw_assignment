using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class Patient_Implementation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Patients",
                newName: "SocialSecurityId");

            migrationBuilder.AlterColumn<string>(
                name: "SocialSecurityId",
                table: "Patients",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "CheckIns",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "HistoryEntry",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PatientId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryEntry_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    HistoryEntryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicines_HistoryEntry_HistoryEntryId",
                        column: x => x.HistoryEntryId,
                        principalTable: "HistoryEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_SocialSecurityId",
                table: "Patients",
                column: "SocialSecurityId",
                unique: true,
                filter: "[SocialSecurityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryEntry_PatientId",
                table: "HistoryEntry",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_HistoryEntryId",
                table: "Medicines",
                column: "HistoryEntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "HistoryEntry");

            migrationBuilder.DropIndex(
                name: "IX_Patients_SocialSecurityId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "CheckIns");

            migrationBuilder.RenameColumn(
                name: "SocialSecurityId",
                table: "Patients",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Patients",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
