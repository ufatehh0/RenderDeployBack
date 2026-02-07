using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeDungeon.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedDataFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Character_Clothing",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Character_ClothingColor",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Character_Emotion",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Character_Gender",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Character_HairColor",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Character_Skin",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash", "Character_Clothing", "Character_ClothingColor", "Character_Emotion", "Character_Gender", "Character_HairColor", "Character_Skin" },
                values: new object[] { new DateTime(2026, 2, 7, 23, 6, 50, 859, DateTimeKind.Utc).AddTicks(820), "$2a$11$Zdwcvlnn52Wda5z/5heS/.wXnOP4FCp8qp1z25HC8/oNsxhZmeP1C", "Default", "None", "Neutral", "None", "None", "Default" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Character_Clothing",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Character_ClothingColor",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Character_Emotion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Character_Gender",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Character_HairColor",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Character_Skin",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 2, 7, 22, 53, 17, 657, DateTimeKind.Utc).AddTicks(8776), "$2a$11$7JN./Yj96eUKCA3Ddtx5XeTOL3QVuff55h7t/9WrHlIt.C9/5H726" });
        }
    }
}
