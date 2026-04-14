using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stutvds.Data.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Triggers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayWordsLimit",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "WordsSpoken",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_ApplicationUserId",
                table: "Triggers",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Triggers_AspNetUsers_ApplicationUserId",
                table: "Triggers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Triggers_AspNetUsers_ApplicationUserId",
                table: "Triggers");

            migrationBuilder.DropIndex(
                name: "IX_Triggers_ApplicationUserId",
                table: "Triggers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Triggers");

            migrationBuilder.DropColumn(
                name: "DayWordsLimit",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WordsSpoken",
                table: "AspNetUsers");
        }
    }
}
