using Microsoft.EntityFrameworkCore.Migrations;

namespace News_Website.Migrations
{
    public partial class quiz3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "QuizResults",
                maxLength: 5000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "QuizResults");
        }
    }
}
