using Microsoft.EntityFrameworkCore.Migrations;

namespace stutvds.Data.Migrations
{
    public partial class AddArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleEntity",
                table: "ArticleEntity");

            migrationBuilder.DropColumn(
                name: "Locale",
                table: "ArticleEntity");

            migrationBuilder.RenameTable(
                name: "ArticleEntity",
                newName: "Articles");

            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "Articles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Articles",
                table: "Articles",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Articles",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Articles");

            migrationBuilder.RenameTable(
                name: "Articles",
                newName: "ArticleEntity");

            migrationBuilder.AddColumn<string>(
                name: "Locale",
                table: "ArticleEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleEntity",
                table: "ArticleEntity",
                column: "Id");
        }
    }
}
