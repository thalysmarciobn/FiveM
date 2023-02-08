using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseConsole.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_character_faceshape");

            migrationBuilder.DropTable(
                name: "account_character_heritage");

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

            migrationBuilder.UpdateData(
                table: "account_character",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTime(2023, 2, 8, 14, 7, 32, 427, DateTimeKind.Local).AddTicks(9852));

            migrationBuilder.InsertData(
                table: "account_character_ped_face",
                columns: new[] { "CharacterId", "Index", "Scale" },
                values: new object[,]
                {
                    { 1L, 19, 0f },
                    { 1L, 18, 0f },
                    { 1L, 16, 0f },
                    { 1L, 15, 0f },
                    { 1L, 14, 0f },
                    { 1L, 13, 0f },
                    { 1L, 12, 0f },
                    { 1L, 11, 0f },
                    { 1L, 10, 0f },
                    { 1L, 17, 0f },
                    { 1L, 8, 0f },
                    { 1L, 9, 0f },
                    { 1L, 1, 0f },
                    { 1L, 2, 0f },
                    { 1L, 3, 0f },
                    { 1L, 0, 0f },
                    { 1L, 5, 0f },
                    { 1L, 6, 0f },
                    { 1L, 7, 0f },
                    { 1L, 4, 0f }
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
                    { 1L, 11, 0, 0f },
                    { 1L, 10, 0, 0f },
                    { 1L, 9, 0, 0f },
                    { 1L, 8, 0, 0f },
                    { 1L, 7, 0, 0f },
                    { 1L, 4, 0, 0f },
                    { 1L, 5, 0, 0f },
                    { 1L, 3, 0, 0f },
                    { 1L, 2, 0, 0f },
                    { 1L, 0, 0, 0f },
                    { 1L, 6, 0, 0f },
                    { 1L, 1, 0, 0f }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_account_character_ped_prop_account_character_CharacterId",
                table: "account_character_ped_prop",
                column: "CharacterId",
                principalTable: "account_character",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_account_character_ped_prop_account_character_CharacterId",
                table: "account_character_ped_prop");

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

            migrationBuilder.CreateTable(
                name: "account_character_faceshape",
                columns: table => new
                {
                    CharacterId = table.Column<long>(type: "bigint", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Scale = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_faceshape", x => x.CharacterId);
                    table.ForeignKey(
                        name: "FK_account_character_faceshape_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_character_heritage",
                columns: table => new
                {
                    CharacterId = table.Column<long>(type: "bigint", nullable: false),
                    IsParent = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ShapeFirstID = table.Column<int>(type: "int", nullable: false),
                    ShapeMix = table.Column<float>(type: "float", nullable: false),
                    ShapeSecondID = table.Column<int>(type: "int", nullable: false),
                    ShapeThirdID = table.Column<int>(type: "int", nullable: false),
                    SkinFirstID = table.Column<int>(type: "int", nullable: false),
                    SkinMix = table.Column<float>(type: "float", nullable: false),
                    SkinSecondID = table.Column<int>(type: "int", nullable: false),
                    SkinThirdID = table.Column<int>(type: "int", nullable: false),
                    ThirdMix = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_character_heritage", x => x.CharacterId);
                    table.ForeignKey(
                        name: "FK_account_character_heritage_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "account_character",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTime(2023, 2, 8, 13, 21, 8, 32, DateTimeKind.Local).AddTicks(277));

            migrationBuilder.InsertData(
                table: "account_character_faceshape",
                columns: new[] { "CharacterId", "Index", "Scale" },
                values: new object[] { 1L, 0, 0f });

            migrationBuilder.InsertData(
                table: "account_character_heritage",
                columns: new[] { "CharacterId", "IsParent", "ShapeFirstID", "ShapeMix", "ShapeSecondID", "ShapeThirdID", "SkinFirstID", "SkinMix", "SkinSecondID", "SkinThirdID", "ThirdMix" },
                values: new object[] { 1L, false, 0, 0f, 0, 0, 0, 0f, 0, 0, 0f });
        }
    }
}
