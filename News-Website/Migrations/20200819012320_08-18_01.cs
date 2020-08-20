using Microsoft.EntityFrameworkCore.Migrations;

namespace News_Website.Migrations
{
    public partial class _0818_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AltText",
                table: "BlobFiles",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BlobFiles",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArticleBlobFile",
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false),
                    BlobFileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleBlobFile", x => new { x.ArticleId, x.BlobFileId });
                    table.ForeignKey(
                        name: "FK_ArticleBlobFile_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleBlobFile_BlobFiles_BlobFileId",
                        column: x => x.BlobFileId,
                        principalTable: "BlobFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleBlobFile_BlobFileId",
                table: "ArticleBlobFile",
                column: "BlobFileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleBlobFile");

            migrationBuilder.DropColumn(
                name: "AltText",
                table: "BlobFiles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BlobFiles");
        }
    }
}
