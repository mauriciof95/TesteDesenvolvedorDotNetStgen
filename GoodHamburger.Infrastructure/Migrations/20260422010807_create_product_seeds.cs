using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GoodHamburger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class create_product_seeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "product",
                columns: new[] { "id", "created_at", "deleted_at", "name", "price", "product_type", "updated_at" },
                values: new object[,]
                {
                    { 1L, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, "X Burger", 5.00m, 0, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, "X Egg", 4.50m, 0, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3L, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, "X Bacon", 7.00m, 0, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4L, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, "Batata frita", 2.00m, 1, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5L, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, "Refrigerante", 2.50m, 2, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "product",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "product",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "product",
                keyColumn: "id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "product",
                keyColumn: "id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "product",
                keyColumn: "id",
                keyValue: 5L);
        }
    }
}
