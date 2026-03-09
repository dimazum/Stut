using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stutvds.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalisis2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VolumeDb",
                table: "VoiceAnalyses",
                newName: "VolumeStdDb");

            migrationBuilder.RenameColumn(
                name: "MeanPitch",
                table: "VoiceAnalyses",
                newName: "PitchStd");

            migrationBuilder.AlterColumn<double>(
                name: "Shimmer",
                table: "VoiceAnalyses",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Jitter",
                table: "VoiceAnalyses",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<double>(
                name: "Duration",
                table: "VoiceAnalyses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PauseRatio",
                table: "VoiceAnalyses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PitchMean",
                table: "VoiceAnalyses",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SpeechRate",
                table: "VoiceAnalyses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VolumeMeanDb",
                table: "VoiceAnalyses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VolumePeakDb",
                table: "VoiceAnalyses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "VoiceAnalyses");

            migrationBuilder.DropColumn(
                name: "PauseRatio",
                table: "VoiceAnalyses");

            migrationBuilder.DropColumn(
                name: "PitchMean",
                table: "VoiceAnalyses");

            migrationBuilder.DropColumn(
                name: "SpeechRate",
                table: "VoiceAnalyses");

            migrationBuilder.DropColumn(
                name: "VolumeMeanDb",
                table: "VoiceAnalyses");

            migrationBuilder.DropColumn(
                name: "VolumePeakDb",
                table: "VoiceAnalyses");

            migrationBuilder.RenameColumn(
                name: "VolumeStdDb",
                table: "VoiceAnalyses",
                newName: "VolumeDb");

            migrationBuilder.RenameColumn(
                name: "PitchStd",
                table: "VoiceAnalyses",
                newName: "MeanPitch");

            migrationBuilder.AlterColumn<double>(
                name: "Shimmer",
                table: "VoiceAnalyses",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Jitter",
                table: "VoiceAnalyses",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
