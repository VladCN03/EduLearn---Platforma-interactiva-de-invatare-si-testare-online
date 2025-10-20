using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalPlatform_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIDToPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Questions",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserID",
                table: "Posts",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_UserID",
                table: "Posts",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserID",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserID",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Questions",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Posts");
        }
    }
}
