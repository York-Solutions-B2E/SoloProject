using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedCommunicationTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CommunicationTypes",
                columns: new[] { "TypeCode", "DisplayName" },
                values: new object[,]
                {
                    { "EOB", "Explanation of Benefits" },
                    { "IDCARD", "ID Card" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommunicationTypes",
                keyColumn: "TypeCode",
                keyValue: "EOB");

            migrationBuilder.DeleteData(
                table: "CommunicationTypes",
                keyColumn: "TypeCode",
                keyValue: "IDCARD");
        }
    }
}
