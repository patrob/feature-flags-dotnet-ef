using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FeatureFlagsEfDemo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeatureDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureDetail_Feature_Id",
                        column: x => x.Id,
                        principalTable: "Feature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Feature",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Subtract" },
                    { 2, "HandleDivideByZeroRequests" }
                });

            migrationBuilder.InsertData(
                table: "FeatureDetail",
                columns: new[] { "Id", "IsEnabled" },
                values: new object[,]
                {
                    { 1, false },
                    { 2, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feature_Name",
                table: "Feature",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureDetail");

            migrationBuilder.DropTable(
                name: "Feature");
        }
    }
}
