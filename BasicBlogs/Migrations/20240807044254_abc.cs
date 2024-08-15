using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicBlogs.Migrations
{
    /// <inheritdoc />
    public partial class abc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "Blogs",
                newName: "Image");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishTime",
                table: "Blogs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishTime",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Blogs",
                newName: "Photo");
        }
    }
}
