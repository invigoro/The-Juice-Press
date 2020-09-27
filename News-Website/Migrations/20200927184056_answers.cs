using Microsoft.EntityFrameworkCore.Migrations;

namespace News_Website.Migrations
{
    public partial class answers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "QuizQuestionAnswers",
                maxLength: 2000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "QuizQuestionAnswers");
        }
    }
}
