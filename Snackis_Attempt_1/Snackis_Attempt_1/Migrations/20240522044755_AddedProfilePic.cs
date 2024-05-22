using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snackis_Attempt_1.Migrations
{
    /// <inheritdoc />
    public partial class AddedProfilePic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePic",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePic",
                table: "Comments");
        }
    }
}
