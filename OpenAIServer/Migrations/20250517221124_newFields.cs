using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAIServer.Migrations
{
    /// <inheritdoc />
    public partial class newFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Level",
                table: "ERStats",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "ERStats",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Deaths",
                table: "ERStats",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "ERStats",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ERStats",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxHP",
                table: "ERStats",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryWeapon",
                table: "ERStats",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Runes",
                table: "ERStats",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryWeapon",
                table: "ERStats",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TertiaryWeapon",
                table: "ERStats",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                table: "ERStats");

            migrationBuilder.DropColumn(
                name: "Deaths",
                table: "ERStats");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "ERStats");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ERStats");

            migrationBuilder.DropColumn(
                name: "MaxHP",
                table: "ERStats");

            migrationBuilder.DropColumn(
                name: "PrimaryWeapon",
                table: "ERStats");

            migrationBuilder.DropColumn(
                name: "Runes",
                table: "ERStats");

            migrationBuilder.DropColumn(
                name: "SecondaryWeapon",
                table: "ERStats");

            migrationBuilder.DropColumn(
                name: "TertiaryWeapon",
                table: "ERStats");

            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "ERStats",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
