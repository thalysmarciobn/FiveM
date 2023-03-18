using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseConsole.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    License = table.Column<string>(nullable: true),
                    CurrentCharacter = table.Column<long>(nullable: false),
                    LastAddress = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    WhiteListed = table.Column<bool>(maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blips",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BlipId = table.Column<int>(nullable: false),
                    DisplayId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Color = table.Column<int>(nullable: false),
                    Scale = table.Column<float>(nullable: false),
                    ShortRange = table.Column<bool>(nullable: false),
                    X = table.Column<float>(nullable: false),
                    Y = table.Column<float>(nullable: false),
                    Z = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "server_vehicle_service",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Model = table.Column<uint>(nullable: false),
                    Driver = table.Column<uint>(nullable: false),
                    Key = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    MarkX = table.Column<float>(nullable: false),
                    MarkY = table.Column<float>(nullable: false),
                    MarkZ = table.Column<float>(nullable: false),
                    SpawnX = table.Column<float>(nullable: false),
                    SpawnY = table.Column<float>(nullable: false),
                    SpawnZ = table.Column<float>(nullable: false),
                    SpawnHeading = table.Column<float>(nullable: false),
                    DriveToX = table.Column<float>(nullable: false),
                    DriveToY = table.Column<float>(nullable: false),
                    DriveToZ = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_server_vehicle_service", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CharacterId = table.Column<long>(nullable: false),
                    Model = table.Column<uint>(nullable: false),
                    Livery = table.Column<int>(nullable: false),
                    BodyHealth = table.Column<float>(nullable: false),
                    DashboardColor = table.Column<int>(nullable: false),
                    DirtLevel = table.Column<float>(nullable: false),
                    DoorLockStatus = table.Column<int>(nullable: false),
                    DoorsLockedForPlayer = table.Column<int>(nullable: false),
                    DoorStatus = table.Column<int>(nullable: false),
                    EngineHealth = table.Column<float>(nullable: false),
                    Handbrake = table.Column<bool>(nullable: false),
                    HeadlightsColour = table.Column<int>(nullable: false),
                    HomingLockonState = table.Column<int>(nullable: false),
                    InteriorColor = table.Column<int>(nullable: false),
                    LightsOn = table.Column<bool>(nullable: false),
                    HighbeamsOn = table.Column<bool>(nullable: false),
                    NumberPlateText = table.Column<string>(nullable: true),
                    NumberPlateTextIndex = table.Column<int>(nullable: false),
                    WheelType = table.Column<int>(nullable: false),
                    WindowTint = table.Column<int>(nullable: false),
                    PrimaryColour = table.Column<int>(nullable: false),
                    SecondaryColour = table.Column<int>(nullable: false),
                    PearlColour = table.Column<int>(nullable: false),
                    WheelColour = table.Column<int>(nullable: false),
                    CustomPrimaryColourR = table.Column<int>(nullable: false),
                    CustomPrimaryColourG = table.Column<int>(nullable: false),
                    CustomPrimaryColourB = table.Column<int>(nullable: false),
                    CustomSecondaryColourR = table.Column<int>(nullable: false),
                    CustomSecondaryColourG = table.Column<int>(nullable: false),
                    CustomSecondaryColourB = table.Column<int>(nullable: false),
                    TyreSmokeColorR = table.Column<int>(nullable: false),
                    TyreSmokeColorG = table.Column<int>(nullable: false),
                    TyreSmokeColorB = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "account_character",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(nullable: false),
                    Slot = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Model = table.Column<string>(nullable: true),
                    Health = table.Column<int>(nullable: false),
                    Armor = table.Column<int>(nullable: false),
                    MoneyBalance = table.Column<int>(nullable: false),
                    BankBalance = table.Column<int>(nullable: false),
                    EyeColorId = table.Column<int>(nullable: false),
                    Heading = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character", x => x.Id);
                    table.ForeignKey(
                        name: "FK_account_character_account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_ped_component",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    ComponentId = table.Column<int>(nullable: false),
                    DrawableId = table.Column<int>(nullable: false),
                    TextureId = table.Column<int>(nullable: false),
                    PalleteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_ped_component", x => new { x.CharacterId, x.ComponentId });
                    table.ForeignKey(
                        name: "FK_account_character_ped_component_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_ped_face",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    Scale = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_ped_face", x => new { x.CharacterId, x.Index });
                    table.ForeignKey(
                        name: "FK_account_character_ped_face_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_ped_head",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    HairColorId = table.Column<int>(nullable: false),
                    HairHighlightColor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_ped_head", x => x.CharacterId);
                    table.ForeignKey(
                        name: "FK_account_character_ped_head_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_ped_head_data",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    ShapeFirstID = table.Column<int>(nullable: false),
                    ShapeSecondID = table.Column<int>(nullable: false),
                    ShapeThirdID = table.Column<int>(nullable: false),
                    SkinFirstID = table.Column<int>(nullable: false),
                    SkinSecondID = table.Column<int>(nullable: false),
                    SkinThirdID = table.Column<int>(nullable: false),
                    ShapeMix = table.Column<float>(nullable: false),
                    SkinMix = table.Column<float>(nullable: false),
                    ThirdMix = table.Column<float>(nullable: false),
                    IsParent = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_ped_head_data", x => x.CharacterId);
                    table.ForeignKey(
                        name: "FK_account_character_ped_head_data_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_ped_head_overlay",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    OverlayId = table.Column<int>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    Opacity = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_ped_head_overlay", x => new { x.CharacterId, x.OverlayId });
                    table.ForeignKey(
                        name: "FK_account_character_ped_head_overlay_account_character_Charact~",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_ped_head_overlay_color",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    OverlayId = table.Column<int>(nullable: false),
                    ColorType = table.Column<int>(nullable: false),
                    ColorId = table.Column<int>(nullable: false),
                    SecondColorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_ped_head_overlay_color", x => new { x.CharacterId, x.OverlayId });
                    table.ForeignKey(
                        name: "FK_account_character_ped_head_overlay_color_account_character_C~",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_ped_prop",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    PropId = table.Column<int>(nullable: false),
                    ComponentId = table.Column<int>(nullable: false),
                    DrawableId = table.Column<int>(nullable: false),
                    TextureId = table.Column<int>(nullable: false),
                    Attach = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_ped_prop", x => new { x.CharacterId, x.PropId });
                    table.ForeignKey(
                        name: "FK_account_character_ped_prop_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_position",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    X = table.Column<float>(nullable: false),
                    Y = table.Column<float>(nullable: false),
                    Z = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_position", x => x.CharacterId);
                    table.ForeignKey(
                        name: "FK_account_character_position_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_rotation",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    X = table.Column<float>(nullable: false),
                    Y = table.Column<float>(nullable: false),
                    Z = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_rotation", x => x.CharacterId);
                    table.ForeignKey(
                        name: "FK_account_character_rotation_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "blips",
                columns: new[] { "Id", "BlipId", "Color", "DisplayId", "Scale", "ShortRange", "Title", "X", "Y", "Z" },
                values: new object[,]
                {
                    { 1L, 198, 70, 4, 0.9f, true, "Terminal de Taxi", -1033.49f, -2727.02f, 13.75f },
                    { 13L, 679, 0, 4, 0.9f, true, "Cassino", 917.37f, 50.76f, 80.76f },
                    { 12L, 410, 46, 4, 0.9f, true, "Terminal Marítimo", 1299.2f, 4217.9f, 33.9f },
                    { 11L, 826, 26, 4, 0.9f, true, "Agência de Taxi", 913.76f, -179.71f, 74.16f },
                    { 10L, 408, 26, 4, 0.9f, true, "Dominos Pizza", 536.19f, 98.79f, 96.44f },
                    { 8L, 50, 0, 4, 0.9f, true, "Estacionamento", -335.82f, 264.83f, 85.89f },
                    { 9L, 50, 0, 4, 0.9f, true, "Estacionamento", 58.3f, -624.23f, 31.66f },
                    { 6L, 50, 0, 4, 0.9f, true, "Estacionamento", -1179.2f, -1507.13f, 4.37f },
                    { 5L, 50, 0, 4, 0.9f, true, "Estacionamento", 606.64f, 73.82f, 91.93f },
                    { 4L, 50, 0, 4, 0.9f, true, "Estacionamento", 208.75f, -808.06f, 30.88f },
                    { 3L, 50, 0, 4, 0.9f, true, "Estacionamento", 44.37f, -865.29f, 30.53f },
                    { 2L, 50, 0, 4, 0.9f, true, "Estacionamento", 98.95f, -1067.59f, 29.29f },
                    { 7L, 50, 0, 4, 0.9f, true, "Estacionamento", -1160.81f, -726.49f, 20.57f }
                });

            migrationBuilder.InsertData(
                table: "server_vehicle_service",
                columns: new[] { "Id", "DriveToX", "DriveToY", "DriveToZ", "Driver", "Key", "MarkX", "MarkY", "MarkZ", "Model", "SpawnHeading", "SpawnX", "SpawnY", "SpawnZ", "Title" },
                values: new object[,]
                {
                    { 3L, -1596.211f, -1044.552f, 12.533f, 1885233650u, 38, -1026.4174f, -2730.4631f, 13.7566f, 3338918751u, 238.933f, -1024.145f, -2728.84f, 13.272f, "Taxi Del Perro" },
                    { 1L, 134.954f, -1023.76f, 28.8165f, 1885233650u, 38, -1049.649f, -2719.027f, 13.7566f, 3338918751u, 240.2623f, -1051.63f, -2712.7f, 14f, "TAXI PRAÇA" },
                    { 2L, 918.015f, 50.655f, 80.247f, 1885233650u, 38, -1041.9746f, -2721.6182f, 13.7566f, 3338918751u, 240.2623f, -1040.689f, -2719.0955f, 13.28f, "Taxi Casino" },
                    { 4L, 0f, 0f, 0f, 1885233650u, 38, -1014.7446f, -2737.0579f, 13.7566f, 3338918751u, 0f, 0f, 0f, 0f, "Chamar Taxi" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_Id_License",
                table: "account",
                columns: new[] { "Id", "License" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_account_character_AccountId",
                table: "account_character",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_account_character_Id_AccountId_Slot",
                table: "account_character",
                columns: new[] { "Id", "AccountId", "Slot" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_Id_CharacterId",
                table: "vehicle",
                columns: new[] { "Id", "CharacterId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_character_ped_component");

            migrationBuilder.DropTable(
                name: "account_character_ped_face");

            migrationBuilder.DropTable(
                name: "account_character_ped_head");

            migrationBuilder.DropTable(
                name: "account_character_ped_head_data");

            migrationBuilder.DropTable(
                name: "account_character_ped_head_overlay");

            migrationBuilder.DropTable(
                name: "account_character_ped_head_overlay_color");

            migrationBuilder.DropTable(
                name: "account_character_ped_prop");

            migrationBuilder.DropTable(
                name: "account_character_position");

            migrationBuilder.DropTable(
                name: "account_character_rotation");

            migrationBuilder.DropTable(
                name: "blips");

            migrationBuilder.DropTable(
                name: "server_vehicle_service");

            migrationBuilder.DropTable(
                name: "vehicle");

            migrationBuilder.DropTable(
                name: "account_character");

            migrationBuilder.DropTable(
                name: "account");
        }
    }
}
