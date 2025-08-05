using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedGlobalStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GlobalStatuses",
                columns: new[] { "StatusCode", "Description" },
                values: new object[,]
                {
                    { "Archived", "Archived" },
                    { "Cancelled", "Cancelled" },
                    { "Delivered", "Delivered" },
                    { "Expired", "Expired" },
                    { "Failed", "Failed" },
                    { "Inserted", "Inserted" },
                    { "InTransit", "In Transit" },
                    { "Printed", "Printed" },
                    { "QueuedForPrinting", "Queued For Printing" },
                    { "ReadyForRelease", "Ready For Release" },
                    { "Released", "Released" },
                    { "Returned", "Returned" },
                    { "Shipped", "Shipped" },
                    { "WarehouseReady", "Warehouse Ready" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GlobalStatuses",
                keyColumn: "StatusCode",
                keyValue: "Archived");

            migrationBuilder.DeleteData(
                table: "GlobalStatuses",
                keyColumn: "StatusCode",
                keyValue: "Cancelled");

            migrationBuilder.DeleteData(
                table: "GlobalStatuses",
                keyColumn: "StatusCode",
                keyValue: "Delivered");

            migrationBuilder.DeleteData(
                table: "GlobalStatuses",
                keyColumn: "StatusCode",
                keyValue: "Expired");

            migrationBuilder.DeleteData(
                table: "GlobalStatuses",
                keyColumn: "StatusCode",
                keyValue: "Failed");

            migrationBuilder.DeleteData(
                table: "GlobalStatuses",
                keyColumn: "StatusCode",
                keyValue: "Inserted");

            migrationBuilder.DeleteData(
                table: "GlobalStatuses",
                keyColumn: "StatusCode",
                keyValue: "InTransit");

            migrationBuilder.DeleteData(
                table: "GlobalStatuses",
                keyColumn: "StatusCode",
                keyValue: "Printed");

            migrationBuilder.DeleteData(
                table: "GlobalStatuses",
                keyColumn: "StatusCode",
                keyValue: "QueuedForPrinting");

            migrationBuilder.DeleteData(
                table: "GlobalStatuses",
                keyColumn: "StatusCode",
                keyValue: "ReadyForRelease");

            migrationBuilder.DeleteData(
                table: "GlobalStatuses",
                keyColumn: "StatusCode",
                keyValue: "Released");

            migrationBuilder.DeleteData(
                table: "GlobalStatuses",
                keyColumn: "StatusCode",
                keyValue: "Returned");

            migrationBuilder.DeleteData(
                table: "GlobalStatuses",
                keyColumn: "StatusCode",
                keyValue: "Shipped");

            migrationBuilder.DeleteData(
                table: "GlobalStatuses",
                keyColumn: "StatusCode",
                keyValue: "WarehouseReady");
        }
    }
}
