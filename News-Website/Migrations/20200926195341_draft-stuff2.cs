using Microsoft.EntityFrameworkCore.Migrations;

namespace News_Website.Migrations
{
    public partial class draftstuff2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DraftCoverImageId",
                table: "Articles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_DraftCoverImageId",
                table: "Articles",
                column: "DraftCoverImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_BlobFiles_DraftCoverImageId",
                table: "Articles",
                column: "DraftCoverImageId",
                principalTable: "BlobFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_BlobFiles_DraftCoverImageId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_DraftCoverImageId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "DraftCoverImageId",
                table: "Articles");
        }
    }
}
