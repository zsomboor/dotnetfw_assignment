using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class InitialAddressImplementationAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HomeAddress_Address",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeAddress_City",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeAddress_Country",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeAddress_Province",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeAddress_Street",
                table: "Patients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeAddress_Address",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "HomeAddress_City",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "HomeAddress_Country",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "HomeAddress_Province",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "HomeAddress_Street",
                table: "Patients");
        }
    }
}
