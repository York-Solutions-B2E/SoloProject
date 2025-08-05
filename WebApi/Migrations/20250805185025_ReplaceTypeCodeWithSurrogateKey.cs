using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceTypeCodeWithSurrogateKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communications_CommunicationTypes_CommunicationTypeTypeCode",
                table: "Communications");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunicationTypeStatuses_CommunicationTypes_TypeCode",
                table: "CommunicationTypeStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunicationTypeStatuses",
                table: "CommunicationTypeStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunicationTypes",
                table: "CommunicationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Communications_CommunicationTypeTypeCode",
                table: "Communications");

            migrationBuilder.DeleteData(
                table: "CommunicationTypes",
                keyColumn: "TypeCode",
                keyValue: "EOB");

            migrationBuilder.DeleteData(
                table: "CommunicationTypes",
                keyColumn: "TypeCode",
                keyValue: "IDCARD");

            migrationBuilder.DropColumn(
                name: "TypeCode",
                table: "CommunicationTypeStatuses");

            migrationBuilder.DropColumn(
                name: "CommunicationTypeTypeCode",
                table: "Communications");

            migrationBuilder.DropColumn(
                name: "TypeCode",
                table: "Communications");

            migrationBuilder.AddColumn<Guid>(
                name: "CommunicationTypeId",
                table: "CommunicationTypeStatuses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "CommunicationTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CommunicationTypeId",
                table: "Communications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunicationTypeStatuses",
                table: "CommunicationTypeStatuses",
                columns: new[] { "CommunicationTypeId", "StatusCode" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunicationTypes",
                table: "CommunicationTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationTypes_TypeCode",
                table: "CommunicationTypes",
                column: "TypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Communications_CommunicationTypeId",
                table: "Communications",
                column: "CommunicationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Communications_CommunicationTypes_CommunicationTypeId",
                table: "Communications",
                column: "CommunicationTypeId",
                principalTable: "CommunicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunicationTypeStatuses_CommunicationTypes_CommunicationTypeId",
                table: "CommunicationTypeStatuses",
                column: "CommunicationTypeId",
                principalTable: "CommunicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communications_CommunicationTypes_CommunicationTypeId",
                table: "Communications");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunicationTypeStatuses_CommunicationTypes_CommunicationTypeId",
                table: "CommunicationTypeStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunicationTypeStatuses",
                table: "CommunicationTypeStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunicationTypes",
                table: "CommunicationTypes");

            migrationBuilder.DropIndex(
                name: "IX_CommunicationTypes_TypeCode",
                table: "CommunicationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Communications_CommunicationTypeId",
                table: "Communications");

            migrationBuilder.DropColumn(
                name: "CommunicationTypeId",
                table: "CommunicationTypeStatuses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CommunicationTypes");

            migrationBuilder.DropColumn(
                name: "CommunicationTypeId",
                table: "Communications");

            migrationBuilder.AddColumn<string>(
                name: "TypeCode",
                table: "CommunicationTypeStatuses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommunicationTypeTypeCode",
                table: "Communications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeCode",
                table: "Communications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunicationTypeStatuses",
                table: "CommunicationTypeStatuses",
                columns: new[] { "TypeCode", "StatusCode" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunicationTypes",
                table: "CommunicationTypes",
                column: "TypeCode");

            migrationBuilder.InsertData(
                table: "CommunicationTypes",
                columns: new[] { "TypeCode", "DisplayName" },
                values: new object[,]
                {
                    { "EOB", "Explanation of Benefits" },
                    { "IDCARD", "ID Card" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Communications_CommunicationTypeTypeCode",
                table: "Communications",
                column: "CommunicationTypeTypeCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Communications_CommunicationTypes_CommunicationTypeTypeCode",
                table: "Communications",
                column: "CommunicationTypeTypeCode",
                principalTable: "CommunicationTypes",
                principalColumn: "TypeCode");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunicationTypeStatuses_CommunicationTypes_TypeCode",
                table: "CommunicationTypeStatuses",
                column: "TypeCode",
                principalTable: "CommunicationTypes",
                principalColumn: "TypeCode",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
