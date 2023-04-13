using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseConsole.Migrations
{
    public partial class Item : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "account_character_item",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Slot",
                table: "account_character_item",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "vehicle_mod",
                columns: table => new
                {
                    VehicleId = table.Column<long>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Index = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_mod", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_vehicle_mod_vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_mod_VehicleId",
                table: "vehicle_mod",
                column: "VehicleId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vehicle_mod");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "account_character_item");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "account_character_item");
        }
    }
}
