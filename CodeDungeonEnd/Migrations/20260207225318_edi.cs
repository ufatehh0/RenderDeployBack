using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeDungeon.Migrations
{
    /// <inheritdoc />
    public partial class edi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 2, 7, 22, 53, 17, 657, DateTimeKind.Utc).AddTicks(8776), "$2a$11$7JN./Yj96eUKCA3Ddtx5XeTOL3QVuff55h7t/9WrHlIt.C9/5H726" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FinCode",
                table: "Users",
                type: "character(7)",
                fixedLength: true,
                maxLength: 7,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "FinCode", "PasswordHash", "PhoneNumber" },
                values: new object[] { new DateTime(2026, 2, 7, 22, 46, 33, 741, DateTimeKind.Utc).AddTicks(6625), "ADMIN12", "$2a$11$80uKUxC0f4UHUK52H7nvi.PhBfI.Hft3nOdKlBKiXnj17eskqwO4C", "+994000000000" });
        }
    }
}
