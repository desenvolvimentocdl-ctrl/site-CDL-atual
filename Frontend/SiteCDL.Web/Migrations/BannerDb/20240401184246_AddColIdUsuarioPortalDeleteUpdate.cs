using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteCDL.Migrations.BannerDb
{
    /// <inheritdoc />
    public partial class AddColIdUsuarioPortalDeleteUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioPortalDelete",
                table: "Banners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioPortalUpdate",
                table: "Banners",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdUsuarioPortalDelete",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "IdUsuarioPortalUpdate",
                table: "Banners");
        }
    }
}
