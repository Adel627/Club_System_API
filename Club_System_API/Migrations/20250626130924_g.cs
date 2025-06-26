using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Club_System_API.Migrations
{
    /// <inheritdoc />
    public partial class g : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembershipEndDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MembershipStartDate",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "JoinedAt",
                value: new DateOnly(2025, 6, 26));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "MembershipEndDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MembershipStartDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "JoinedAt", "MembershipEndDate", "MembershipStartDate" },
                values: new object[] { new DateOnly(2025, 6, 24), null, null });
        }
    }
}
