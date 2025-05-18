using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Club_System_API.Migrations
{
    /// <inheritdoc />
    public partial class AddMembershipImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Memberships",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Memberships");
        }
    }
}
