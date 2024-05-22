using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snackis_Attempt_1.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPostCreatorInPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostCreator",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostCreator",
                table: "Posts");
        }
    }
}
