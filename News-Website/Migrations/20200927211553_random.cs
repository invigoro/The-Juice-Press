using Microsoft.EntityFrameworkCore.Migrations;

namespace News_Website.Migrations
{
    public partial class random : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizResponses_QuizResults_QuizResultId",
                table: "QuizResponses");

            migrationBuilder.DropIndex(
                name: "IX_QuizResponses_QuizResultId",
                table: "QuizResponses");

            migrationBuilder.AddColumn<bool>(
                name: "RandomQuestionOrder",
                table: "Quizzes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "AnswerResultWeights",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RandomQuestionOrder",
                table: "Quizzes");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "AnswerResultWeights",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_QuizResponses_QuizResultId",
                table: "QuizResponses",
                column: "QuizResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResponses_QuizResults_QuizResultId",
                table: "QuizResponses",
                column: "QuizResultId",
                principalTable: "QuizResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
