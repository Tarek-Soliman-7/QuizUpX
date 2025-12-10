using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Choices");

            migrationBuilder.RenameColumn(
                name: "answers",
                table: "Questions",
                newName: "choices");

            migrationBuilder.AlterColumn<int>(
                name: "correctIndex",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: -1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "choices",
                table: "Questions",
                newName: "answers");

            migrationBuilder.AlterColumn<int>(
                name: "correctIndex",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: -1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Choices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    OptionNumber = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Choices_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Choices_QuestionId_OptionNumber",
                table: "Choices",
                columns: new[] { "QuestionId", "OptionNumber" },
                unique: true);
        }
    }
}
