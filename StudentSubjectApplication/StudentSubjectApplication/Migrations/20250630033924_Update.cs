using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentSubjectApplication.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Students_AssignedStudentsid",
                table: "StudentSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Subjects_AssignedSubjectsid",
                table: "StudentSubject");

            migrationBuilder.RenameColumn(
                name: "AssignedSubjectsid",
                table: "StudentSubject",
                newName: "subjectsid");

            migrationBuilder.RenameColumn(
                name: "AssignedStudentsid",
                table: "StudentSubject",
                newName: "studentsid");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubject_AssignedSubjectsid",
                table: "StudentSubject",
                newName: "IX_StudentSubject_subjectsid");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Students_studentsid",
                table: "StudentSubject",
                column: "studentsid",
                principalTable: "Students",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Subjects_subjectsid",
                table: "StudentSubject",
                column: "subjectsid",
                principalTable: "Subjects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Students_studentsid",
                table: "StudentSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Subjects_subjectsid",
                table: "StudentSubject");

            migrationBuilder.RenameColumn(
                name: "subjectsid",
                table: "StudentSubject",
                newName: "AssignedSubjectsid");

            migrationBuilder.RenameColumn(
                name: "studentsid",
                table: "StudentSubject",
                newName: "AssignedStudentsid");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubject_subjectsid",
                table: "StudentSubject",
                newName: "IX_StudentSubject_AssignedSubjectsid");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Students_AssignedStudentsid",
                table: "StudentSubject",
                column: "AssignedStudentsid",
                principalTable: "Students",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Subjects_AssignedSubjectsid",
                table: "StudentSubject",
                column: "AssignedSubjectsid",
                principalTable: "Subjects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
