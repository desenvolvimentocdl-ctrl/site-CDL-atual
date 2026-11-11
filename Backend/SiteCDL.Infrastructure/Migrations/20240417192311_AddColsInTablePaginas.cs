using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteCDL.Migrations
{
    /// <inheritdoc />
    public partial class AddColsInTablePaginas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioPortalCreate",
                table: "Paginas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioPortalDelete",
                table: "Paginas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioPortalUpdate",
                table: "Paginas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Paginas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdUsuarioPortalCreate",
                table: "Paginas");

            migrationBuilder.DropColumn(
                name: "IdUsuarioPortalDelete",
                table: "Paginas");

            migrationBuilder.DropColumn(
                name: "IdUsuarioPortalUpdate",
                table: "Paginas");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Paginas");
        }
    }
}
