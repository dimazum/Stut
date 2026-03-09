using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stutvds.Data.Migrations
{
    /// <inheritdoc />
    public partial class DayLessonFinishTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "DayLessons",
                newName: "StartTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishTime",
                table: "DayLessons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PauseTime",
                table: "DayLessons",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishTime",
                table: "DayLessons");

            migrationBuilder.DropColumn(
                name: "PauseTime",
                table: "DayLessons");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "DayLessons",
                newName: "Date");
        }
    }
}
