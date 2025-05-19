using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAIServer.Migrations
{
    /// <inheritdoc />
    public partial class newFields3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "EventList",
                table: "ERStats",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Events",
                table: "ERStats",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventList",
                table: "ERStats");

            migrationBuilder.DropColumn(
                name: "Events",
                table: "ERStats");
        }
    }
}
