using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeDungeon.Migrations
{
    /// <inheritdoc />
    public partial class addlevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "Level", "PasswordHash" },
                values: new object[] { new DateTime(2026, 2, 8, 0, 0, 19, 206, DateTimeKind.Utc).AddTicks(3387), 100, "$2a$11$jSwooIdCM5gymmsCCyogZuJjq6EadFfaQG.mcj/r4v8bH3LUGROaG" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 2, 7, 23, 6, 50, 859, DateTimeKind.Utc).AddTicks(820), "$2a$11$Zdwcvlnn52Wda5z/5heS/.wXnOP4FCp8qp1z25HC8/oNsxhZmeP1C" });
        }
    }
}
