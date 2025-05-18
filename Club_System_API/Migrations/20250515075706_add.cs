using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Club_System_API.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

        

        

        

            migrationBuilder.AddColumn<DateTime>(
                name: "MembershipEndDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MembershipId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MembershipStartDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

           
            migrationBuilder.CreateTable(
                name: "MembershipPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MembershipId = table.Column<int>(type: "int", nullable: false),
                    StripeSessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipPayments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MembershipPayments_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "JoinedAt", "MembershipEndDate", "MembershipId", "MembershipStartDate" },
                values: new object[] { new DateOnly(2025, 5, 15), null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MembershipId",
                table: "AspNetUsers",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPayments_MembershipId",
                table: "MembershipPayments",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPayments_UserId",
                table: "MembershipPayments",
                column: "UserId");


            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Memberships_MembershipId",
                table: "AspNetUsers",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Memberships_MembershipId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MembershipPayments");

            migrationBuilder.DropTable(
                name: "UserMemberships");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MembershipId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "MembershipEndDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MembershipId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MembershipStartDate",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "JoinedAt",
                value: new DateOnly(2025, 5, 2));
        }
    }
}
