using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stutvds.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalisis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VoiceAnalyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    MeanPitch = table.Column<double>(type: "float", nullable: true),
                    PitchMin = table.Column<double>(type: "float", nullable: true),
                    PitchMax = table.Column<double>(type: "float", nullable: true),
                    VolumeDb = table.Column<double>(type: "float", nullable: false),
                    MfccJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoiceAnalyses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoiceAnalyses");
        }
    }
}
