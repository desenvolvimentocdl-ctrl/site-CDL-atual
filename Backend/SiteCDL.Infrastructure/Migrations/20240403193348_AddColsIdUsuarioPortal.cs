using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteCDL.Migrations
{
    /// <inheritdoc />
    public partial class AddColsIdUsuarioPortal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioPortalCreate",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioPortalDelete",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioPortalUpdate",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdUsuarioPortalCreate",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "IdUsuarioPortalDelete",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "IdUsuarioPortalUpdate",
                table: "Post");
        }
    }
}
