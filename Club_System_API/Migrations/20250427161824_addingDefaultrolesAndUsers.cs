using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Club_System_API.Migrations
{
    /// <inheritdoc />
    public partial class addingDefaultrolesAndUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MembershipNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "92b75286-d8f8-4061-9995-e6e23ccdee94", "f51e5a91-bced-49c2-8b86-c2e170c0846c", "Admin", "ADMIN" },
                    { "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4", "5ee6bc12-5cb0-4304-91e7-6a00744e042a", "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Birth_Of_Date", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "Image", "IsDisabled", "JoinedAt", "LastName", "LockoutEnabled", "LockoutEnd", "MembershipNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Renewal_date", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6dc6528a-b280-4770-9eae-82671ee81ef7", 0, new DateOnly(2001, 12, 20), "99d2bbc6-bc54-4248-a172-a77de3ae4430", null, false, "Club System", null, false, new DateOnly(2025, 4, 27), "Admin", false, null, "Admin7", null, "ADMIN7", "AQAAAAIAAYagAAAAEG11fXlkvcGxeUaPMcHd42YNq9TZKK3UfeGAXgrhHVQTIaDq50veaZghpOpmlPQ4cg==", "01120443096", true, new DateOnly(2026, 4, 27), "55BF92C9EF0249CDA210D85D1A851BC9", false, "Admin7" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "92b75286-d8f8-4061-9995-e6e23ccdee94", "6dc6528a-b280-4770-9eae-82671ee81ef7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "92b75286-d8f8-4061-9995-e6e23ccdee94", "6dc6528a-b280-4770-9eae-82671ee81ef7" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92b75286-d8f8-4061-9995-e6e23ccdee94");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7");

            migrationBuilder.AlterColumn<string>(
                name: "MembershipNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
