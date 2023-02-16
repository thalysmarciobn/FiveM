using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseConsole.Migrations
{
    public partial class vehicles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "server_vehicle_service");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "server_vehicle_service");

            migrationBuilder.DropColumn(
                name: "Z",
                table: "server_vehicle_service");

            migrationBuilder.AddColumn<float>(
                name: "DriveToX",
                table: "server_vehicle_service",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DriveToY",
                table: "server_vehicle_service",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DriveToZ",
                table: "server_vehicle_service",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Key",
                table: "server_vehicle_service",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "MarkX",
                table: "server_vehicle_service",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MarkY",
                table: "server_vehicle_service",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MarkZ",
                table: "server_vehicle_service",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SpawnX",
                table: "server_vehicle_service",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SpawnY",
                table: "server_vehicle_service",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SpawnZ",
                table: "server_vehicle_service",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "server_vehicle_service",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Key", "MarkX", "MarkY", "MarkZ", "Title" },
                values: new object[] { 38, -1049.649f, -2719.027f, 13.7566f, "Chamar Taxi" });

            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Key", "MarkX", "MarkY", "MarkZ", "Title" },
                values: new object[] { 38, -1041.9746f, -2721.6182f, 13.7566f, "Chamar Taxi" });

            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Key", "MarkX", "MarkY", "MarkZ", "Title" },
                values: new object[] { 38, -1026.4174f, -2730.4631f, 13.7566f, "Chamar Taxi" });

            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Key", "MarkX", "MarkY", "MarkZ", "Title" },
                values: new object[] { 38, -1014.7446f, -2737.0579f, 13.7566f, "Chamar Taxi" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriveToX",
                table: "server_vehicle_service");

            migrationBuilder.DropColumn(
                name: "DriveToY",
                table: "server_vehicle_service");

            migrationBuilder.DropColumn(
                name: "DriveToZ",
                table: "server_vehicle_service");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "server_vehicle_service");

            migrationBuilder.DropColumn(
                name: "MarkX",
                table: "server_vehicle_service");

            migrationBuilder.DropColumn(
                name: "MarkY",
                table: "server_vehicle_service");

            migrationBuilder.DropColumn(
                name: "MarkZ",
                table: "server_vehicle_service");

            migrationBuilder.DropColumn(
                name: "SpawnX",
                table: "server_vehicle_service");

            migrationBuilder.DropColumn(
                name: "SpawnY",
                table: "server_vehicle_service");

            migrationBuilder.DropColumn(
                name: "SpawnZ",
                table: "server_vehicle_service");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "server_vehicle_service");

            migrationBuilder.AddColumn<float>(
                name: "X",
                table: "server_vehicle_service",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Y",
                table: "server_vehicle_service",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Z",
                table: "server_vehicle_service",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "X", "Y", "Z" },
                values: new object[] { -1049.649f, -2719.027f, 13.7566f });

            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "X", "Y", "Z" },
                values: new object[] { -1041.9746f, -2721.6182f, 13.7566f });

            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "X", "Y", "Z" },
                values: new object[] { -1026.4174f, -2730.4631f, 13.7566f });

            migrationBuilder.UpdateData(
                table: "server_vehicle_service",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "X", "Y", "Z" },
                values: new object[] { -1014.7446f, -2737.0579f, 13.7566f });
        }
    }
}
