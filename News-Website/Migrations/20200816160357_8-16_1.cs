using Microsoft.EntityFrameworkCore.Migrations;

namespace News_Website.Migrations
{
    public partial class _816_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Articles",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(55)",
                oldMaxLength: 55,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlShortCode",
                table: "Articles",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Articles_UrlShortCode",
                table: "Articles",
                column: "UrlShortCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Articles_UrlShortCode",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "UrlShortCode",
                table: "Articles");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Articles",
                type: "character varying(55)",
                maxLength: 55,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
