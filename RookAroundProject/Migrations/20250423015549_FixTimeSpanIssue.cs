using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RookAround.Migrations
{
    /// <inheritdoc />
    public partial class FixTimeSpanIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime_New",
                table: "Players",
                type: "interval",
                nullable: true);
                
            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime_New",
                table: "Players",
                type: "interval",
                nullable: true);
            
            // Set default values for the new columns (you might want to adjust this)
            migrationBuilder.Sql("UPDATE \"Players\" SET \"StartTime_New\" = '00:00:00', \"EndTime_New\" = '23:59:59' WHERE \"PlayerType\" = 'GMPlayer'");
            
            // Drop old columns
            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Players");
                
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Players");
            
            // Rename new columns to original names
            migrationBuilder.RenameColumn(
                name: "StartTime_New",
                table: "Players",
                newName: "StartTime");
                
            migrationBuilder.RenameColumn(
                name: "EndTime_New",
                table: "Players",
                newName: "EndTime");
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPlayers_Tournaments_TournamentId",
                table: "TournamentPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Venues_VenueId",
                table: "Tournaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentPlayers",
                table: "TournamentPlayers");

            migrationBuilder.DropIndex(
                name: "IX_TournamentPlayers_TournamentId",
                table: "TournamentPlayers");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Availabilities",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "TournamentId",
                table: "TournamentPlayers",
                newName: "ParticipatingTournamentsId");

            migrationBuilder.AlterColumn<int>(
                name: "VenueId",
                table: "Tournaments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "Players",
                type: "interval",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "Players",
                type: "interval",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentPlayers",
                table: "TournamentPlayers",
                columns: new[] { "ParticipatingTournamentsId", "PlayersId" });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPlayers_PlayersId",
                table: "TournamentPlayers",
                column: "PlayersId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPlayers_Tournaments_ParticipatingTournamentsId",
                table: "TournamentPlayers",
                column: "ParticipatingTournamentsId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Venues_VenueId",
                table: "Tournaments",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime_Old",
                table: "Players",
                type: "timestamp with time zone",
                nullable: true);
                
            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime_Old",
                table: "Players",
                type: "timestamp with time zone",
                nullable: true);
            
            // Set some default values for the DateTime columns
            migrationBuilder.Sql("UPDATE \"Players\" SET \"StartTime_Old\" = '2025-01-01', \"EndTime_Old\" = '2025-01-01' WHERE \"PlayerType\" = 'GMPlayer'");
            
            // Drop the TimeSpan columns
            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Players");
                
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Players");
            
            // Rename the DateTime columns to the original names
            migrationBuilder.RenameColumn(
                name: "StartTime_Old",
                table: "Players",
                newName: "StartTime");
                
            migrationBuilder.RenameColumn(
                name: "EndTime_Old",
                table: "Players",
                newName: "EndTime");
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPlayers_Tournaments_ParticipatingTournamentsId",
                table: "TournamentPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Venues_VenueId",
                table: "Tournaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentPlayers",
                table: "TournamentPlayers");

            migrationBuilder.DropIndex(
                name: "IX_TournamentPlayers_PlayersId",
                table: "TournamentPlayers");

            migrationBuilder.RenameColumn(
                name: "ParticipatingTournamentsId",
                table: "TournamentPlayers",
                newName: "TournamentId");

            migrationBuilder.AlterColumn<int>(
                name: "VenueId",
                table: "Tournaments",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "Tournaments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Players",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Players",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "Availabilities",
                table: "Players",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Players",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentPlayers",
                table: "TournamentPlayers",
                columns: new[] { "PlayersId", "TournamentId" });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPlayers_TournamentId",
                table: "TournamentPlayers",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPlayers_Tournaments_TournamentId",
                table: "TournamentPlayers",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Venues_VenueId",
                table: "Tournaments",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
