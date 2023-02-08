using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseConsole.Migrations
{
    public partial class FaceShape : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account_character_faceshape",
                columns: table => new
                {
                    CharacterId = table.Column<long>(nullable: false),
                    NoseWidth = table.Column<float>(nullable: false),
                    NosePeakHeight = table.Column<float>(nullable: false),
                    NosePeakLength = table.Column<float>(nullable: false),
                    NoseBoneHeight = table.Column<float>(nullable: false),
                    NosePeakLowering = table.Column<float>(nullable: false),
                    NoseBoneTwist = table.Column<float>(nullable: false),
                    EyeBrowHeight = table.Column<float>(nullable: false),
                    EyeBrowLength = table.Column<float>(nullable: false),
                    CheekBoneHeight = table.Column<float>(nullable: false),
                    CheekBoneWidth = table.Column<float>(nullable: false),
                    CheekWidth = table.Column<float>(nullable: false),
                    EyeOpenings = table.Column<float>(nullable: false),
                    LipThickness = table.Column<float>(nullable: false),
                    JawBoneWidth = table.Column<float>(nullable: false),
                    JawBoneLength = table.Column<float>(nullable: false),
                    ChinBoneLowering = table.Column<float>(nullable: false),
                    ChinBoneLength = table.Column<float>(nullable: false),
                    ChinBoneWidth = table.Column<float>(nullable: false),
                    ChinDimple = table.Column<float>(nullable: false),
                    NeckThickness = table.Column<float>(nullable: false)
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

            migrationBuilder.UpdateData(
                table: "account_character",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTime(2023, 2, 8, 9, 8, 8, 355, DateTimeKind.Local).AddTicks(6776));

            migrationBuilder.InsertData(
                table: "account_character_faceshape",
                columns: new[] { "CharacterId", "CheekBoneHeight", "CheekBoneWidth", "CheekWidth", "ChinBoneLength", "ChinBoneLowering", "ChinBoneWidth", "ChinDimple", "EyeBrowHeight", "EyeBrowLength", "EyeOpenings", "JawBoneLength", "JawBoneWidth", "LipThickness", "NeckThickness", "NoseBoneHeight", "NoseBoneTwist", "NosePeakHeight", "NosePeakLength", "NosePeakLowering", "NoseWidth" },
                values: new object[] { 1L, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_character_faceshape");

            migrationBuilder.UpdateData(
                table: "account_character",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTime(2023, 2, 8, 9, 6, 40, 774, DateTimeKind.Local).AddTicks(9250));
        }
    }
}
