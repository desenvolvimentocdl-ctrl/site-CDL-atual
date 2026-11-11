using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteCDL.Migrations.BannerDb
{
    /// <inheritdoc />
    public partial class AddColumNomeTableBanners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Banners",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Banners");
        }
    }
}
