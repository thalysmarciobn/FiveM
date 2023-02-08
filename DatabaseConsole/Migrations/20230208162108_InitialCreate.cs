using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseConsole.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account_character",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    License = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    WhiteListed = table.Column<bool>(maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "account_character_faceshape",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    Scale = table.Column<float>(nullable: false)
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
                    table.PrimaryKey("PK_account_character_heritage", x => x.CharacterId);
                    table.ForeignKey(
                        name: "FK_account_character_heritage_account_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "account_character",
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
                table: "account_character",
                columns: new[] { "Id", "AccountId", "Armor", "DateCreated", "Gender", "Model", "Name", "Surname" },
                values: new object[] { 1L, 1L, 0, new DateTime(2023, 2, 8, 13, 21, 8, 32, DateTimeKind.Local).AddTicks(277), 0, "mp_m_freemode_01", "Admin", "Thalys" });

            migrationBuilder.InsertData(
                table: "account_character_ped_prop",
                columns: new[] { "CharacterId", "PropId", "Index", "Texture" },
                values: new object[,]
                {
                    { 1L, 0, 0, 0 },
                    { 1L, 1, 0, 0 },
                    { 1L, 2, 0, 0 },
                    { 1L, 3, 0, 0 },
                    { 1L, 4, 0, 0 },
                    { 1L, 5, 0, 0 },
                    { 1L, 6, 0, 0 },
                    { 1L, 7, 0, 0 },
                    { 1L, 8, 0, 0 },
                    { 1L, 9, 0, 0 }
                });

            migrationBuilder.InsertData(
                table: "account_character_faceshape",
                columns: new[] { "CharacterId", "Index", "Scale" },
                values: new object[] { 1L, 0, 0f });

            migrationBuilder.InsertData(
                table: "account_character_heritage",
                columns: new[] { "CharacterId", "IsParent", "ShapeFirstID", "ShapeMix", "ShapeSecondID", "ShapeThirdID", "SkinFirstID", "SkinMix", "SkinSecondID", "SkinThirdID", "ThirdMix" },
                values: new object[] { 1L, false, 0, 0f, 0, 0, 0, 0f, 0, 0, 0f });

            migrationBuilder.InsertData(
                table: "account_character_ped_component",
                columns: new[] { "CharacterId", "ComponentId", "Index", "Texture" },
                values: new object[,]
                {
                    { 1L, 0, 0, 0 },
                    { 1L, 1, 0, 0 },
                    { 1L, 2, 0, 0 },
                    { 1L, 3, 0, 0 },
                    { 1L, 4, 0, 0 },
                    { 1L, 5, 0, 0 },
                    { 1L, 6, 0, 0 },
                    { 1L, 7, 0, 0 },
                    { 1L, 8, 0, 0 },
                    { 1L, 9, 0, 0 },
                    { 1L, 10, 0, 0 },
                    { 1L, 11, 0, 0 }
                });

            migrationBuilder.InsertData(
                table: "account_character_position",
                columns: new[] { "ChatacterId", "X", "Y", "Z" },
                values: new object[] { 1L, 0f, 0f, 0f });

            migrationBuilder.CreateIndex(
                name: "IX_account_character_Id_AccountId",
                table: "account_character",
                columns: new[] { "Id", "AccountId" });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_License",
                table: "accounts",
                column: "License",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accounts_Id_License",
                table: "accounts",
                columns: new[] { "Id", "License" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_character_faceshape");

            migrationBuilder.DropTable(
                name: "account_character_heritage");

            migrationBuilder.DropTable(
                name: "account_character_ped_component");

            migrationBuilder.DropTable(
                name: "account_character_ped_prop");

            migrationBuilder.DropTable(
                name: "account_character_position");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "account_character");
        }
    }
}
