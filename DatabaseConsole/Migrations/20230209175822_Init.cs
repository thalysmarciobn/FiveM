using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseConsole.Migrations
{
    public partial class Init : Migration
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
                    Created = table.Column<DateTime>(nullable: false),
                    WhiteListed = table.Column<bool>(maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.Id);
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
                    Gender = table.Column<int>(nullable: false),
                    Armor = table.Column<int>(nullable: false),
                    Model = table.Column<string>(nullable: true)
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
                    Index = table.Column<int>(nullable: false),
                    Texture = table.Column<int>(nullable: false)
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
                    EyeColorId = table.Column<int>(nullable: false),
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
                    ColortId = table.Column<int>(nullable: false),
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
                    Index = table.Column<int>(nullable: false),
                    Texture = table.Column<int>(nullable: false)
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
                    ChatacterId = table.Column<long>(nullable: false),
                    X = table.Column<float>(nullable: false),
                    Y = table.Column<float>(nullable: false),
                    Z = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_position", x => x.ChatacterId);
                    table.ForeignKey(
                        name: "FK_account_character_position_account_character_ChatacterId",
                        column: x => x.ChatacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "account",
                columns: new[] { "Id", "Created", "License", "WhiteListed" },
                values: new object[] { 1L, new DateTime(2023, 2, 9, 14, 58, 22, 91, DateTimeKind.Local).AddTicks(9039), "07041d870811cccd5a93a5a012970b341d168b9a", true });

            migrationBuilder.InsertData(
                table: "account_character",
                columns: new[] { "Id", "AccountId", "Armor", "DateCreated", "Gender", "Model", "Name", "Slot", "Surname" },
                values: new object[] { 1L, 1L, 0, new DateTime(2023, 2, 9, 14, 58, 22, 109, DateTimeKind.Local).AddTicks(9757), 0, "mp_m_freemode_01", "Admin", 0, "Thalys" });

            migrationBuilder.InsertData(
                table: "account_character_ped_component",
                columns: new[] { "CharacterId", "ComponentId", "Index", "Texture" },
                values: new object[,]
                {
                    { 1L, 0, 0, 0 },
                    { 1L, 11, 0, 0 },
                    { 1L, 9, 0, 0 },
                    { 1L, 8, 0, 0 },
                    { 1L, 7, 0, 0 },
                    { 1L, 6, 0, 0 },
                    { 1L, 10, 0, 0 },
                    { 1L, 4, 0, 0 },
                    { 1L, 3, 0, 0 },
                    { 1L, 2, 0, 0 },
                    { 1L, 1, 0, 0 },
                    { 1L, 5, 0, 0 }
                });

            migrationBuilder.InsertData(
                table: "account_character_ped_face",
                columns: new[] { "CharacterId", "Index", "Scale" },
                values: new object[,]
                {
                    { 1L, 11, 0f },
                    { 1L, 19, 0f },
                    { 1L, 18, 0f },
                    { 1L, 17, 0f },
                    { 1L, 15, 0f },
                    { 1L, 14, 0f },
                    { 1L, 13, 0f },
                    { 1L, 12, 0f },
                    { 1L, 10, 0f },
                    { 1L, 16, 0f },
                    { 1L, 8, 0f },
                    { 1L, 0, 0f },
                    { 1L, 2, 0f },
                    { 1L, 3, 0f },
                    { 1L, 1, 0f },
                    { 1L, 5, 0f },
                    { 1L, 6, 0f },
                    { 1L, 7, 0f },
                    { 1L, 4, 0f },
                    { 1L, 9, 0f }
                });

            migrationBuilder.InsertData(
                table: "account_character_ped_head",
                columns: new[] { "CharacterId", "EyeColorId", "HairColorId", "HairHighlightColor" },
                values: new object[] { 1L, 0, 0, 0 });

            migrationBuilder.InsertData(
                table: "account_character_ped_head_data",
                columns: new[] { "CharacterId", "IsParent", "ShapeFirstID", "ShapeMix", "ShapeSecondID", "ShapeThirdID", "SkinFirstID", "SkinMix", "SkinSecondID", "SkinThirdID", "ThirdMix" },
                values: new object[] { 1L, false, 0, 0f, 0, 0, 0, 0f, 0, 0, 0f });

            migrationBuilder.InsertData(
                table: "account_character_ped_head_overlay",
                columns: new[] { "CharacterId", "OverlayId", "Index", "Opacity" },
                values: new object[,]
                {
                    { 1L, 12, 0, 0f },
                    { 1L, 7, 0, 0f },
                    { 1L, 11, 0, 0f },
                    { 1L, 10, 0, 0f },
                    { 1L, 9, 0, 0f },
                    { 1L, 8, 0, 0f },
                    { 1L, 5, 0, 0f },
                    { 1L, 4, 0, 0f },
                    { 1L, 3, 0, 0f },
                    { 1L, 2, 0, 0f },
                    { 1L, 1, 0, 0f },
                    { 1L, 0, 0, 0f },
                    { 1L, 6, 0, 0f }
                });

            migrationBuilder.InsertData(
                table: "account_character_ped_prop",
                columns: new[] { "CharacterId", "PropId", "Index", "Texture" },
                values: new object[,]
                {
                    { 1L, 8, 0, 0 },
                    { 1L, 7, 0, 0 },
                    { 1L, 6, 0, 0 },
                    { 1L, 5, 0, 0 },
                    { 1L, 2, 0, 0 },
                    { 1L, 3, 0, 0 },
                    { 1L, 1, 0, 0 },
                    { 1L, 0, 0, 0 },
                    { 1L, 9, 0, 0 },
                    { 1L, 4, 0, 0 }
                });

            migrationBuilder.InsertData(
                table: "account_character_position",
                columns: new[] { "ChatacterId", "X", "Y", "Z" },
                values: new object[] { 1L, 0f, 0f, 0f });

            migrationBuilder.CreateIndex(
                name: "IX_account_Id_License",
                table: "account",
                columns: new[] { "Id", "License" });

            migrationBuilder.CreateIndex(
                name: "IX_account_character_AccountId",
                table: "account_character",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_account_character_Id_AccountId",
                table: "account_character",
                columns: new[] { "Id", "AccountId" });
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
                name: "account_character");

            migrationBuilder.DropTable(
                name: "account");
        }
    }
}
