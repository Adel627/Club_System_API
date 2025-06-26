using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Club_System_API.Migrations
{
    /// <inheritdoc />
    public partial class addclubreviewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RatedAt",
                table: "CoachReviews",
                newName: "ReviewAt");

            migrationBuilder.CreateTable(
                name: "clubReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ReviewAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clubReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_clubReviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "JoinedAt",
                value: new DateOnly(2025, 6, 24));

            migrationBuilder.CreateIndex(
                name: "IX_clubReviews_UserId",
                table: "clubReviews",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clubReviews");

            migrationBuilder.RenameColumn(
                name: "ReviewAt",
                table: "CoachReviews",
                newName: "RatedAt");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "JoinedAt",
                value: new DateOnly(2025, 6, 23));
        }
    }
}
