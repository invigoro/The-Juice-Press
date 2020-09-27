using Microsoft.EntityFrameworkCore.Migrations;

namespace News_Website.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestionAnswers_QuizQuestions_QuestionId",
                table: "QuizQuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestions_Quizzes_QuizId",
                table: "QuizQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizResults_Quizzes_QuizId",
                table: "QuizResults");

            migrationBuilder.DropIndex(
                name: "IX_QuizQuestionAnswers_QuestionId",
                table: "QuizQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "QuizQuestionAnswers");

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "QuizResults",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "QuizQuestions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuizQuestionId",
                table: "QuizQuestionAnswers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestionAnswers_QuizQuestionId",
                table: "QuizQuestionAnswers",
                column: "QuizQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestionAnswers_QuizQuestions_QuizQuestionId",
                table: "QuizQuestionAnswers",
                column: "QuizQuestionId",
                principalTable: "QuizQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestions_Quizzes_QuizId",
                table: "QuizQuestions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "QuizId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResults_Quizzes_QuizId",
                table: "QuizResults",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "QuizId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestionAnswers_QuizQuestions_QuizQuestionId",
                table: "QuizQuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestions_Quizzes_QuizId",
                table: "QuizQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizResults_Quizzes_QuizId",
                table: "QuizResults");

            migrationBuilder.DropIndex(
                name: "IX_QuizQuestionAnswers_QuizQuestionId",
                table: "QuizQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "QuizQuestionId",
                table: "QuizQuestionAnswers");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Quizzes",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "QuizResults",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "QuizQuestions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "QuizQuestionAnswers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestionAnswers_QuestionId",
                table: "QuizQuestionAnswers",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestionAnswers_QuizQuestions_QuestionId",
                table: "QuizQuestionAnswers",
                column: "QuestionId",
                principalTable: "QuizQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestions_Quizzes_QuizId",
                table: "QuizQuestions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "QuizId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResults_Quizzes_QuizId",
                table: "QuizResults",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "QuizId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
