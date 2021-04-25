using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsidWebExercise.Migrations
{
    public partial class updatedlibraryitems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryItems_Categories_CategoryId",
                table: "LibraryItems");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "LibraryItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryItems_Categories_CategoryId",
                table: "LibraryItems",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryItems_Categories_CategoryId",
                table: "LibraryItems");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "LibraryItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryItems_Categories_CategoryId",
                table: "LibraryItems",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
