using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class RemovePrescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_HistoryEntry_HistoryEntryId",
                table: "Medicines");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_HistoryEntryId",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "HistoryEntryId",
                table: "Medicines");

            migrationBuilder.AddColumn<long>(
                name: "CheckInId",
                table: "HistoryEntry",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckInId",
                table: "HistoryEntry");

            migrationBuilder.AddColumn<long>(
                name: "HistoryEntryId",
                table: "Medicines",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_HistoryEntryId",
                table: "Medicines",
                column: "HistoryEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_HistoryEntry_HistoryEntryId",
                table: "Medicines",
                column: "HistoryEntryId",
                principalTable: "HistoryEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
