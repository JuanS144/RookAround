using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RookAround.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Tournaments_TournamentId1",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_TournamentId1",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TournamentId1",
                table: "Matches");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Players",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Elo",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Pwd",
                table: "Players",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Players",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Elo",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Pwd",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "TournamentId1",
                table: "Matches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentId1",
                table: "Matches",
                column: "TournamentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Tournaments_TournamentId1",
                table: "Matches",
                column: "TournamentId1",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
