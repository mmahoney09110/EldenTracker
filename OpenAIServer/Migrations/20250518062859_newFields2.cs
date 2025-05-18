using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAIServer.Migrations
{
    /// <inheritdoc />
    public partial class newFields2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GreatRune",
                table: "ERStats",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HP",
                table: "ERStats",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GreatRune",
                table: "ERStats");

            migrationBuilder.DropColumn(
                name: "HP",
                table: "ERStats");
        }
    }
}
