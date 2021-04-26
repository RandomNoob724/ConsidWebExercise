using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsidWebExercise.Migrations
{
    public partial class libraryitemfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Barrower",
                table: "LibraryItems",
                newName: "Borrower");

            migrationBuilder.RenameColumn(
                name: "BarrowDate",
                table: "LibraryItems",
                newName: "BorrowDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Borrower",
                table: "LibraryItems",
                newName: "Barrower");

            migrationBuilder.RenameColumn(
                name: "BorrowDate",
                table: "LibraryItems",
                newName: "BarrowDate");
        }
    }
}
