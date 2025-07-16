using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bi_dashboard_api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeedDataFromModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "Name", "Status" },
                values: new object[,]
                {
                    { 1, "john.doe@example.com", "John Doe", "Active" },
                    { 2, "jane.smith@example.com", "Jane Smith", "Active" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "Completed", "CustomerId", "Placed", "Total" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2024, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 150.99m },
                    { 2, new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2024, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 89.50m }
                });
        }
    }
}
