using Microsoft.AspNetCore.Builder;
using Microsoft.Graph.ApplicationsWithAppId;
using Microsoft.IdentityModel.Tokens;
using StudentSubjectApplication.Domain.Entities;
using StudentSubjectApplication.Domain.Repositories;
using StudentSubjectApplication.Presentation.DTOs;
using System;

namespace StudentSubjectApplication.Presentation.Controller
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
            studentGroup.MapGet("/GetByName/{name}", GetStudentByName);
            studentGroup.MapPost("/Add", AddStudent);
            studentGroup.MapPut("/Update/{id}", UpdateStudent);
            studentGroup.MapDelete("/Delete/{id}", DeleteStudent);
            studentGroup.MapGet("/GetRelatedSubjects/{id}", GetRelatedSubjects);

            subjectGroup.MapGet("/GetAll", GetAllSubjects);
            subjectGroup.MapGet("/GetById/{id}", GetSubjectById);
            subjectGroup.MapGet("/GetByName/{name}", GetSubjectByName);
            subjectGroup.MapPost("/Add", AddSubject);
            subjectGroup.MapPut("/Update/{id}", UpdateSubject);
            subjectGroup.MapDelete("/Delete/{id}", DeleteSubject);
            subjectGroup.MapGet("/GetRelatedStudents/{id}", GetRelatedStudents);
            subjectGroup.MapPost("/assignStudent/{subjectId}/{studentId}", AssignStudent);
            subjectGroup.MapDelete("/UnassignStudent/{subjectId}/{studentId}", UnassignStudent);


            static async Task<IResult> GetAllStudents(IGenericRepository<Student, Subject> repository)
            {
                try
                {
                    var students = await repository.GetAllAsync();
                    List<StudentDTO> studentDTOs = new List<StudentDTO>();

                    foreach (var student in students)
                    {
                        var studentDTO = new StudentDTO(student.id, student.name, student.dateOfBirth, student.age, student.address);
                        studentDTOs.Add(studentDTO);
                    }

                    return Results.Ok(studentDTOs);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

            }

                    
            static async Task<IResult> GetStudentById(string id, IGenericRepository<Student, Subject> repository)
            {
                try
                {


                    var student = await repository.GetByIdAsync(id);
                    if (student != null)
                    {
                        var studentDTO = new StudentDTO(student.id, student.name, student.dateOfBirth, student.age, student.address);
                        return Results.Ok(studentDTO);
                    }
                    else
                        return Results.NotFound("Student is Not Found");
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

            }

            static async Task<IResult> GetStudentByName(string name, IGenericRepository<Student,Subject> repository)
            {
                try
                {
                    var student = await repository.GetByNameAsync(name);
                    if (student != null)
                    {
                        var studentDTO = new StudentDTO(student.id, student.name, student.dateOfBirth, student.age, student.address);
                        return Results.Ok(studentDTO);
                    }
                    else
                        return Results.NotFound("Student is Not Found");
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

            }

            static async Task<IResult> AddStudent(StudentAddUpdateDTO studentDTO, IGenericRepository<Student, Subject> repository)
            {
                try
                {
                    List<Student> existingStudents = await repository.GetAllAsync();
                    int studentIdSeed = existingStudents.Count();
                    if (studentIdSeed == 0)
                    {
                        studentIdSeed = 0;
                    }
                    else
                    {
                        Student lastStudent = existingStudents.OrderByDescending(s => s.id).FirstOrDefault();
                        if (lastStudent != null)
                        {
                            studentIdSeed = int.Parse(lastStudent.id.Substring(2));
                        }
                    }
                    var studentId = "ST" + (studentIdSeed + 1);
                    var dateOfBirth = DateOnly.Parse(studentDTO.dateOfBirth);
                    var student = new Student(studentId, studentDTO.name, studentDTO.age, dateOfBirth, studentDTO.address);
                    repository.AddAsync(student);
                    return Results.Created("", new StudentDTO(student.id, student.name, student.dateOfBirth, student.age, student.address));
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

            }

            static async Task<IResult> UpdateStudent(string id, StudentAddUpdateDTO studentDTO, IGenericRepository<Student, Subject> repository)
            {
                try
                {
                    var student = await repository.GetByIdAsync(id);
                    if (student != null)
                    {
                        student.name = studentDTO.name;
                        student.age = studentDTO.age;
                        student.dateOfBirth = DateOnly.Parse(studentDTO.dateOfBirth);
                        student.address = studentDTO.address;
                        await repository.UpdateAsync(student);
                        return Results.Ok(new StudentDTO(id, student.name, student.dateOfBirth, student.age, student.address));
                    }
                    else
                    {
                        return Results.NotFound("Student is Not Found");
                    }
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }

            static async Task<IResult> DeleteStudent(string id, IGenericRepository<Student, Subject> repository)
            {
                try
                {
                    var student = await repository.GetByIdAsync(id);
                    if (student != null)
                    {
                        await repository.DeleteAsync(student);
                        return Results.Ok(new { message = "Student deleted successfully." });
                    }
                    else
                    {
                        return Results.NotFound("Student is Not Found");
                    }
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }

            static async Task<IResult> GetRelatedSubjects(string id, IGenericRepository<Student, Subject> repository)
            {
                try
                {
                    var student = await repository.GetByIdAsync(id);
                    if (student == null)
                    {
                        return Results.NotFound("Student not found.");
                    }
                    var subjects = await repository.GetRelatedEntitiesAsync(student);
                    List<SubjectDTO> subjectDTOs = new List<SubjectDTO>();

                    foreach (var subject in subjects)
                    {
                        var subjectDTO = new SubjectDTO(subject.id, subject.name);
                        subjectDTOs.Add(subjectDTO);
                    }

                    return Results.Ok(subjectDTOs);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }

            static async Task<IResult> GetAllSubjects(IGenericRepository<Subject, Student> repository)
            {
                try
                {
                    var subjects = await repository.GetAllAsync();
                    List<SubjectDTO> subjectDTOs = new List<SubjectDTO>();

                    foreach (var subject in subjects)
                    {
                        var subjectDTO = new SubjectDTO(subject.id, subject.name);
                        subjectDTOs.Add(subjectDTO);
                    }

                    return Results.Ok(subjectDTOs);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

            }

            static async Task<IResult> GetSubjectById(string id, IGenericRepository<Subject, Student> repository)
            {
                try
                {

                    var subject = await repository.GetByIdAsync(id);
                    if (subject != null)
                    {
                        var subjectDTO = new SubjectDTO(subject.id, subject.name);
                        return Results.Ok(subjectDTO);
                    }
                    else
                        return Results.NotFound("Subject is Not Found");
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

            }

            static async Task<IResult> GetSubjectByName(string name, IGenericRepository<Subject, Student> repository)
            {
                try
                {
                    var subject = await repository.GetByNameAsync(name);
                    if (subject != null)
                    {
                        var subjectDTO = new SubjectDTO(subject.id, subject.name);
                        return Results.Ok(subjectDTO);
                    }
                    else
                        return Results.NotFound("Subject is Not Found");
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

            }

            static async Task<IResult> AddSubject(SubjectAddUpdateDTO subjectDTO, IGenericRepository<Subject, Student> repository)
            {
                try
                {
                    List<Subject> existingSubjects = await repository.GetAllAsync();
                    int subjectIdSeed = existingSubjects.Count();
                    if (subjectIdSeed == 0)
                    {
                        subjectIdSeed = 0;
                    }
                    else
                    {
                        Subject lastSubject = existingSubjects.OrderByDescending(s => s.id).FirstOrDefault();
                        if (lastSubject != null)
                        {
                            subjectIdSeed = int.Parse(lastSubject.id.Substring(3));
                        }
                    }
                    var subjectId = "SUB" + (subjectIdSeed + 1);
                    var subject = new Subject(subjectId, subjectDTO.name);
                    repository.AddAsync(subject);
                    return Results.Created("", new SubjectDTO(subject.id, subject.name));
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

            }


            static async Task<IResult> UpdateSubject(string id, SubjectAddUpdateDTO subjectDTO, IGenericRepository<Subject, Student> repository)
            {
                try
                {
                    var subject = await repository.GetByIdAsync(id);
                    if (subject != null)
                    {
                        subject.name = subjectDTO.name;
                        await repository.UpdateAsync(subject);
                        return Results.Ok(new SubjectDTO(id, subject.name));
                    }
                    else
                    {
                        return Results.NotFound("Subject is Not Found");
                    }
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }

            static async Task<IResult> DeleteSubject(string id, IGenericRepository<Subject, Student> repository)
            {
                try
                {
                    var subject = await repository.GetByIdAsync(id);
                    if (subject != null)
                    {
                        await repository.DeleteAsync(subject);
                        return Results.Ok(new { message = "Subject deleted successfully." });
                    }
                    else
                    {
                        return Results.NotFound("Subject is Not Found");
                    }
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }

            static async Task<IResult> GetRelatedStudents(string id, IGenericRepository<Subject, Student> repository)
            {
                try
                {
                    var subject = await repository.GetByIdAsync(id);
                    if (subject == null)
                    {
                        return Results.NotFound("Subject not found.");
                    }
                    var students = await repository.GetRelatedEntitiesAsync(subject);
                    List<StudentDTO> studentDTOs = new List<StudentDTO>();

                    foreach (var student in students)
                    {
                        var studentDTO = new StudentDTO(student.id, student.name, student.dateOfBirth, student.age, student.address);
                        studentDTOs.Add(studentDTO);
                    }

                    return Results.Ok(studentDTOs);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }

            static async Task<IResult> AssignStudent(string studentId, string subjectId, IGenericRepository<Student, Subject> studentRepository, IGenericRepository<Subject, Student> subjectRepository)
            {
                try
                {
                    var student = await studentRepository.GetByIdAsync(studentId);
                    var subject = await subjectRepository.GetByIdAsync(subjectId);

                    if (student == null || subject == null)
                    {
                        return Results.NotFound("Student or Subject not found.");
                    }

                    if (student.relatedEntities == null)
                    {
                        student.relatedEntities = new List<Subject>();
                    }

                    if (!student.relatedEntities.Any(s => s.id == subject.id))
                    {
                        student.relatedEntities.Add(subject);
                        await studentRepository.UpdateAsync(student);
                    }
                    else
                    {
                        return Results.NotFound("Student alraedy assigned");
                    }

                    return Results.Ok(new { message = "Student assigned to subject successfully." });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }

            static async Task<IResult> UnassignStudent(string studentId, string subjectId, IGenericRepository<Student, Subject> studentRepository, IGenericRepository<Subject, Student> subjectRepository)
            {
                try
                {
                    var student = await studentRepository.GetByIdAsync(studentId);
                    var subject = await subjectRepository.GetByIdAsync(subjectId);
                    if (student == null || subject == null)
                    {
                        return Results.NotFound("Student or Subject not found.");
                    }
                    if (student.relatedEntities != null && student.relatedEntities.Any(s => s.id == subject.id))
                    {
                        student.relatedEntities.Remove(subject);
                        await studentRepository.UpdateAsync(student);
                        return Results.Ok(new { message = "Student unassigned from subject successfully." });
                    }
                    else
                    {
                        return Results.NotFound("Student is not assigned to this subject.");
                    }
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }

        }

    }
}
