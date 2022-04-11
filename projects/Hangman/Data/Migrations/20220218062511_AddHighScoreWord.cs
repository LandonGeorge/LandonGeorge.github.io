using Microsoft.EntityFrameworkCore.Migrations;

namespace hangman.Migrations
{
    public partial class AddHighScoreWord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Word",
                table: "HighScores",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Word",
                table: "HighScores");
        }
    }
}
