using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stutvds.Data.Migrations
{
    /// <inheritdoc />
    public partial class Test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Histograms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Char = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Air = table.Column<int>(type: "int", nullable: false),
                    HistogramId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharItems_Histograms_HistogramId",
                        column: x => x.HistogramId,
                        principalTable: "Histograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharItems_HistogramId",
                table: "CharItems",
                column: "HistogramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharItems");

            migrationBuilder.DropTable(
                name: "Histograms");
        }
    }
}
