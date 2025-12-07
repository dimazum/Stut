using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stutvds.Data.Migrations
{
    /// <inheritdoc />
    public partial class DayLessonFinishTime3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LessonTimeInSec",
                table: "DayLessons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessonTimeInSec",
                table: "DayLessons");
        }
    }
}
