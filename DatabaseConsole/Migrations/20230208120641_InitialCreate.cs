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
                values: new object[] { 1L, 1L, 0, new DateTime(2023, 2, 8, 9, 6, 40, 774, DateTimeKind.Local).AddTicks(9250), 0, "mp_m_freemode_01", "Admin", "Thalys" });

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
                name: "account_character_position");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "account_character");
        }
    }
}
