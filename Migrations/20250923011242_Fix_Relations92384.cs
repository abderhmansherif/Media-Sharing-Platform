using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeatBox.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Relations92384 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Is2F",
                table: "AspNetUsers",
                newName: "HasAuthenticator");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasAuthenticator",
                table: "AspNetUsers",
                newName: "Is2F");
        }
    }
}
