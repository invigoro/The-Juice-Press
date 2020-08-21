using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace News_Website.Migrations
{
    public partial class _0820_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OverwrittenOn",
                table: "Articles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverwrittenOn",
                table: "Articles");
        }
    }
}
