using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunicationTypes",
                columns: table => new
                {
                    TypeCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationTypes", x => x.TypeCode);
                });

            migrationBuilder.CreateTable(
                name: "Communications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommunicationTypeTypeCode = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communications_CommunicationTypes_CommunicationTypeTypeCode",
                        column: x => x.CommunicationTypeTypeCode,
                        principalTable: "CommunicationTypes",
                        principalColumn: "TypeCode");
                });

            migrationBuilder.CreateTable(
                name: "CommunicationTypeStatuses",
                columns: table => new
                {
                    TypeCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationTypeStatuses", x => new { x.TypeCode, x.StatusCode });
                    table.ForeignKey(
                        name: "FK_CommunicationTypeStatuses_CommunicationTypes_TypeCode",
                        column: x => x.TypeCode,
                        principalTable: "CommunicationTypes",
                        principalColumn: "TypeCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationStatusHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommunicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccurredUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationStatusHistories_Communications_CommunicationId",
                        column: x => x.CommunicationId,
                        principalTable: "Communications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Communications_CommunicationTypeTypeCode",
                table: "Communications",
                column: "CommunicationTypeTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationStatusHistories_CommunicationId",
                table: "CommunicationStatusHistories",
                column: "CommunicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunicationStatusHistories");

            migrationBuilder.DropTable(
                name: "CommunicationTypeStatuses");

            migrationBuilder.DropTable(
                name: "Communications");

            migrationBuilder.DropTable(
                name: "CommunicationTypes");
        }
    }
}
