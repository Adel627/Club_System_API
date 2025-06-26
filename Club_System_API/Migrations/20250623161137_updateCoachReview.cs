using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Club_System_API.Migrations
{
    /// <inheritdoc />
    public partial class updateCoachReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoachRatings");

            migrationBuilder.CreateTable(
                name: "CoachReviews",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CoachId = table.Column<int>(type: "int", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    RatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachReviews", x => new { x.CoachId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CoachReviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoachReviews_Coachs_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coachs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoachReviews_UserId",
                table: "CoachReviews",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoachReviews");

            migrationBuilder.CreateTable(
                name: "CoachRatings",
                columns: table => new
                {
                    CoachId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachRatings", x => new { x.CoachId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CoachRatings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoachRatings_Coachs_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coachs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoachRatings_UserId",
                table: "CoachRatings",
                column: "UserId");
        }
    }
}
