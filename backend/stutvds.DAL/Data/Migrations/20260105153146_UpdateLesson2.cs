using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stutvds.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLesson2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Rewarded",
                table: "DayLessons",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rewarded",
                table: "DayLessons");
        }
    }
}
