using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseConsole.Migrations
{
    public partial class update_driver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Driver",
                value: 1885233650u);

            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Driver",
                value: 1885233650u);

            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Driver",
                value: 1885233650u);

            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Driver",
                value: 1885233650u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Driver",
                value: 1302784073u);

            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Driver",
                value: 1302784073u);

            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Driver",
                value: 1302784073u);

            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Driver",
                value: 1302784073u);
        }
    }
}
