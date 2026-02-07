using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeDungeon.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPasswordToBCrypt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 2, 7, 22, 46, 33, 741, DateTimeKind.Utc).AddTicks(6625), "$2a$11$80uKUxC0f4UHUK52H7nvi.PhBfI.Hft3nOdKlBKiXnj17eskqwO4C" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 2, 7, 22, 42, 30, 184, DateTimeKind.Utc).AddTicks(3332), "AQAAAAIAAYagAAAAED1n9vNY2rWnjQX2WSmQmzuBWJZ3KnXyCKKBuuazTqTMqqF8pksV49y6kcfXOCnfXg==" });
        }
    }
}
