using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace News_Website.Migrations
{
    public partial class quiz1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    QuizId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UrlShortCode = table.Column<string>(maxLength: 10, nullable: true),
                    Title = table.Column<string>(maxLength: 1000, nullable: true),
                    DraftTitle = table.Column<string>(maxLength: 1000, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    DraftContent = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    EditedOn = table.Column<DateTime>(nullable: false),
                    PublishedOn = table.Column<DateTime>(nullable: true),
                    OverwrittenOn = table.Column<DateTime>(nullable: true),
                    Published = table.Column<bool>(nullable: false),
                    TotalViews = table.Column<int>(nullable: false),
                    Category = table.Column<int>(nullable: true),
                    CoverImageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.QuizId);
                    table.ForeignKey(
                        name: "FK_Quizzes_BlobFiles_CoverImageId",
                        column: x => x.CoverImageId,
                        principalTable: "BlobFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuizAuthors",
                columns: table => new
                {
                    QuizId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    IsPrimaryAuthor = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAuthors", x => new { x.QuizId, x.UserId });
                    table.ForeignKey(
                        name: "FK_QuizAuthors_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizAuthors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizBlobFile",
                columns: table => new
                {
                    QuizId = table.Column<int>(nullable: false),
                    BlobFileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizBlobFile", x => new { x.QuizId, x.BlobFileId });
                    table.ForeignKey(
                        name: "FK_QuizBlobFile_BlobFiles_BlobFileId",
                        column: x => x.BlobFileId,
                        principalTable: "BlobFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizBlobFile_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Order = table.Column<int>(nullable: false),
                    QuizId = table.Column<int>(nullable: true),
                    Question = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizQuestions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuizResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuizId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizResults_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestionAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizQuestionAnswers_QuizQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "QuizQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnswerResultWeights",
                columns: table => new
                {
                    QuizQuestionAnswerId = table.Column<int>(nullable: false),
                    QuizResultId = table.Column<int>(nullable: false),
                    Weight = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerResultWeights", x => new { x.QuizQuestionAnswerId, x.QuizResultId });
                    table.ForeignKey(
                        name: "FK_AnswerResultWeights_QuizQuestionAnswers_QuizQuestionAnswerId",
                        column: x => x.QuizQuestionAnswerId,
                        principalTable: "QuizQuestionAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerResultWeights_QuizResults_QuizResultId",
                        column: x => x.QuizResultId,
                        principalTable: "QuizResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerResultWeights_QuizResultId",
                table: "AnswerResultWeights",
                column: "QuizResultId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAuthors_UserId",
                table: "QuizAuthors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizBlobFile_BlobFileId",
                table: "QuizBlobFile",
                column: "BlobFileId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestionAnswers_QuestionId",
                table: "QuizQuestionAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_QuizId",
                table: "QuizQuestions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizResults_QuizId",
                table: "QuizResults",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_CoverImageId",
                table: "Quizzes",
                column: "CoverImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerResultWeights");

            migrationBuilder.DropTable(
                name: "QuizAuthors");

            migrationBuilder.DropTable(
                name: "QuizBlobFile");

            migrationBuilder.DropTable(
                name: "QuizQuestionAnswers");

            migrationBuilder.DropTable(
                name: "QuizResults");

            migrationBuilder.DropTable(
                name: "QuizQuestions");

            migrationBuilder.DropTable(
                name: "Quizzes");
        }
    }
}
