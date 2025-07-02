using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using StudentSubjectApplication.Domain.Entities;
using StudentSubjectApplication.Domain.Repositories;

namespace StudentSubjectApplication.Presentation
{
    public static class Endpoints
    {
        public static void MapEndpoints(this WebApplication app)
        {
            
            //helloorld
            app.MapGet("/", () => "Hello World!");

            var studentGroup = app.MapGroup("/students");
            var subjectGroup = app.MapGroup("/subjects");

            studentGroup.MapGet("/GetAll", GetAllStudents);
            studentGroup.MapGet("/GetById/{id}", GetStudentById);
            //studentGroup.MapGet("/GetByName/{name}", GetStudentByName);
            //studentGroup.MapPost("/Add", AddStudent);
            //studentGroup.MapPut("/Update", UpdateStudent);
            //studentGroup.MapDelete("/Delete/{id}", DeleteStudent);
            //studentGroup.MapGet("/GetRelatedSubjects/{id}", GetRelatedSubjects);

            //subjectGroup.MapGet("/GetAll", GetAllSubjects);
            //subjectGroup.MapGet("/GetById/{id}", GetSubjectById);
            //subjectGroup.MapGet("/GetByName/{name}", GetSubjectByName);
            //subjectGroup.MapPost("/Add", AddSubject);
            //subjectGroup.MapPut("/Update", UpdateSubject);
            //subjectGroup.MapDelete("/Delete/{id}", DeleteSubject);
            //subjectGroup.MapGet("/GetRelatedSubjects/{id}", GetRelatedSubjects);
            //subjectGroup.MapPost("/assignStudent", AssignStudent);
            //subjectGroup.MapDelete("/removeStudent", RemoveStudent);




            static async Task<IResult> GetAllStudents(IGenericRepository<Student, Subject> repository)
            {
                var students = await repository.GetAllAsync();
                return Results.Ok(students);
            }

            static async Task<IResult> GetStudentById(string id, IGenericRepository<Student, Subject> repository)
            {
                var student = await repository.GetByIdAsync(id);
                return student != null ? Results.Ok(student) : Results.NotFound();
            }

        }

    }
}
