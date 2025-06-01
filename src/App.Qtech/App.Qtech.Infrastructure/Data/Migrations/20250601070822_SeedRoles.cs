using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace App.Qtech.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2abb9af4-8e1b-49b6-b9e9-5266d0b9209c"), "5/31/2025 1:02:03 AM", "Admin", "ADMIN" },
                    { new Guid("6a27de7a-0004-430d-b1a1-7db2bcceadc1"), "5/31/2025 1:02:04 AM", "Accountant", "ACCOUNTANT" },
                    { new Guid("ef7c185f-99d6-452c-8c14-f07d18485738"), "5/31/2025 1:02:05 AM", "Viewer", "VIEWER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2abb9af4-8e1b-49b6-b9e9-5266d0b9209c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a27de7a-0004-430d-b1a1-7db2bcceadc1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ef7c185f-99d6-452c-8c14-f07d18485738"));
        }
    }
}
