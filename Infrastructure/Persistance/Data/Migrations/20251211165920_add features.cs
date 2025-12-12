using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class addfeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuizAppCode",
                table: "Students",
                newName: "Pin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pin",
                table: "Students",
                newName: "QuizAppCode");
        }
    }
}
