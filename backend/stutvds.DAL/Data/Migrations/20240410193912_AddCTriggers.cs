using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace stutvds.Data.Migrations
{
    public partial class AddCTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(maxLength: 200000, nullable: true),
                    Locale = table.Column<string>(nullable: true),
                    AgeGroup = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Triggers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Language = table.Column<int>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: true),
                    TriggerType = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    Difficulty = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Triggers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleEntity");

            migrationBuilder.DropTable(
                name: "Triggers");
        }
    }
}
