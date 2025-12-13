using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorQuizUpx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Subjects_subjectId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Attempts_UserId",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FailedAttempts",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "LastPinResetAt",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PinHash",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AnswersJson",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "CorrectCount",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "DeviceInfo",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "UniversityCode",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Attempts");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Subjects",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Subjects",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Subjects",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Questions",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "subjectId",
                table: "Questions",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "questionType",
                table: "Questions",
                newName: "QuestionType");

            migrationBuilder.RenameColumn(
                name: "mark",
                table: "Questions",
                newName: "Mark");

            migrationBuilder.RenameColumn(
                name: "correctIndex",
                table: "Questions",
                newName: "CorrectIndex");

            migrationBuilder.RenameColumn(
                name: "choices",
                table: "Questions",
                newName: "Choices");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Questions",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_subjectId",
                table: "Questions",
                newName: "IX_Questions_SubjectId");

            migrationBuilder.RenameColumn(
                name: "TotalQuestions",
                table: "Attempts",
                newName: "TotalScore");

            migrationBuilder.RenameColumn(
                name: "TimeTakenSeconds",
                table: "Attempts",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Attempts",
                newName: "CorrectAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "Pin",
                table: "Students",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmittedAt",
                table: "Attempts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "AttemptAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttemptId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    SelectedIndex = table.Column<int>(type: "int", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttemptAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttemptAnswers_Attempts_AttemptId",
                        column: x => x.AttemptId,
                        principalTable: "Attempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttemptAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attempts_StudentId",
                table: "Attempts",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptAnswers_AttemptId",
                table: "AttemptAnswers",
                column: "AttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptAnswers_QuestionId",
                table: "AttemptAnswers",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attempts_Students_StudentId",
                table: "Attempts",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Subjects_SubjectId",
                table: "Questions",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attempts_Students_StudentId",
                table: "Attempts");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Subjects_SubjectId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "AttemptAnswers");

            migrationBuilder.DropIndex(
                name: "IX_Attempts_StudentId",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Subjects",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Subjects",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Subjects",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Questions",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Questions",
                newName: "subjectId");

            migrationBuilder.RenameColumn(
                name: "QuestionType",
                table: "Questions",
                newName: "questionType");

            migrationBuilder.RenameColumn(
                name: "Mark",
                table: "Questions",
                newName: "mark");

            migrationBuilder.RenameColumn(
                name: "CorrectIndex",
                table: "Questions",
                newName: "correctIndex");

            migrationBuilder.RenameColumn(
                name: "Choices",
                table: "Questions",
                newName: "choices");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Questions",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_SubjectId",
                table: "Questions",
                newName: "IX_Questions_subjectId");

            migrationBuilder.RenameColumn(
                name: "TotalScore",
                table: "Attempts",
                newName: "TotalQuestions");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Attempts",
                newName: "TimeTakenSeconds");

            migrationBuilder.RenameColumn(
                name: "CorrectAnswers",
                table: "Attempts",
                newName: "Score");

            migrationBuilder.AlterColumn<string>(
                name: "Pin",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<int>(
                name: "FailedAttempts",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPinResetAt",
                table: "Students",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutEnd",
                table: "Students",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PinHash",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmittedAt",
                table: "Attempts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnswersJson",
                table: "Attempts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CorrectCount",
                table: "Attempts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DeviceInfo",
                table: "Attempts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Attempts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxScore",
                table: "Attempts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UniversityCode",
                table: "Attempts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Attempts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attempts_UserId",
                table: "Attempts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Subjects_subjectId",
                table: "Questions",
                column: "subjectId",
                principalTable: "Subjects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
