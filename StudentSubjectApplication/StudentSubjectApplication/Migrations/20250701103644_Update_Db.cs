using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentSubjectApplication.Migrations
{
    /// <inheritdoc />
    public partial class Update_Db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                newName: "relatedEntitiesid1");

            migrationBuilder.RenameColumn(
                name: "studentsid",
                table: "StudentSubject",
                newName: "relatedEntitiesid");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubject_subjectsid",
                table: "StudentSubject",
                newName: "IX_StudentSubject_relatedEntitiesid1");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Students_relatedEntitiesid",
                table: "StudentSubject",
                column: "relatedEntitiesid",
                principalTable: "Students",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Subjects_relatedEntitiesid1",
                table: "StudentSubject",
                column: "relatedEntitiesid1",
                principalTable: "Subjects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Students_relatedEntitiesid",
                table: "StudentSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Subjects_relatedEntitiesid1",
                table: "StudentSubject");

            migrationBuilder.RenameColumn(
                name: "relatedEntitiesid1",
                table: "StudentSubject",
                newName: "subjectsid");

            migrationBuilder.RenameColumn(
                name: "relatedEntitiesid",
                table: "StudentSubject",
                newName: "studentsid");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubject_relatedEntitiesid1",
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
    }
}
