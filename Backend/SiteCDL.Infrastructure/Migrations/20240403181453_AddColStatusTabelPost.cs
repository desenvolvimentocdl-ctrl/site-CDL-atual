using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteCDL.Migrations
{
    /// <inheritdoc />
    public partial class AddColStatusTabelPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Post",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Post");
        }
    }
}
