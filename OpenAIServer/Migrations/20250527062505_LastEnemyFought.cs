using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAIServer.Migrations
{
    /// <inheritdoc />
    public partial class LastEnemyFought : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastEnemyFought",
                table: "ERStats",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEnemyFought",
                table: "ERStats");
        }
    }
}
