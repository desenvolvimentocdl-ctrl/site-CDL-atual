using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteCDL.Migrations
{
    /// <inheritdoc />
    public partial class CreateRelacionamentoPostCateg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostCategoriaCodigo",
                table: "Post",
                type: "int",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_Post_PostCategoriaCodigo",
                table: "Post",
                column: "PostCategoriaCodigo");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_PostCategoria_PostCategoriaCodigo",
                table: "Post",
                column: "PostCategoriaCodigo",
                principalTable: "PostCategoria",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_PostCategoria_PostCategoriaCodigo",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_PostCategoriaCodigo",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "PostCategoriaCodigo",
                table: "Post");
        }
    }
}
