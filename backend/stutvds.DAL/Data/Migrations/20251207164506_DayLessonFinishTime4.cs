using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stutvds.Data.Migrations
{
    /// <inheritdoc />
    public partial class DayLessonFinishTime4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PauseTime",
                table: "DayLessons");

            migrationBuilder.RenameColumn(
                name: "LessonTimeInSec",
                table: "DayLessons",
                newName: "LeftInSec");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FinishTime",
                table: "DayLessons",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartRangeTime",
                table: "DayLessons",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartRangeTime",
                table: "DayLessons");

            migrationBuilder.RenameColumn(
                name: "LeftInSec",
                table: "DayLessons",
                newName: "LessonTimeInSec");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FinishTime",
                table: "DayLessons",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PauseTime",
                table: "DayLessons",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
