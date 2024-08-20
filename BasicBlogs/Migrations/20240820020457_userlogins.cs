using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicBlogs.Migrations
{
    /// <inheritdoc />
    public partial class userlogins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_UserNameOrEmail",
                table: "UserAccounts");

            migrationBuilder.RenameColumn(
                name: "UserNameOrEmail",
                table: "UserAccounts",
                newName: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "UserAccounts",
                newName: "UserNameOrEmail");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UserNameOrEmail",
                table: "UserAccounts",
                column: "UserNameOrEmail",
                unique: true);
        }
    }
}
