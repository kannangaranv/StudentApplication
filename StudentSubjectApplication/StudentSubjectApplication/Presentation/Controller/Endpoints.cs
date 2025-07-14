using Microsoft.AspNetCore.Builder;
using Microsoft.Graph.ApplicationsWithAppId;
using Microsoft.Graph.Education.Classes.Item.Assignments.Item.Submissions.Item.Return;
using Microsoft.Identity.Client;
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
            var tenantId = Environment.GetEnvironmentVariable("TENANT_ID");
            var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
            var redirectUri = Environment.GetEnvironmentVariable("REDIRECT_URI");
            var scope = Environment.GetEnvironmentVariable("APPLICATION_ID_URI");
            var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");

            app.MapGet("/getAccessToken", async (HttpContext context) =>
            {
                var code = context.Request.Query["code"].ToString();
                var apiAppId = Environment.GetEnvironmentVariable("APPLICATION_API_ID"); 
                if (string.IsNullOrEmpty(code))
                    return Results.BadRequest("Missing 'code' query parameter.");

                var tokenEndpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

                using var httpClient = new HttpClient();

                var formData = new Dictionary<string, string>
                {
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "scope", scope },
                    { "code", code },
                    { "redirect_uri", redirectUri },
                    { "grant_type", "authorization_code" },
                    { "session_state", clientId } 
                };

                var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
                {
                    Content = new FormUrlEncodedContent(formData)
                };
                
                var response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                return Results.Content(content, "application/json");
            });

            // Define the groups for students and subjects
            var studentGroup = app.MapGroup("/students");
            var subjectGroup = app.MapGroup("/subjects");

            // Map the endpoints for students and subjects
            studentGroup.MapGet("/GetAll", GetAllStudents).RequireAuthorization();
            studentGroup.MapGet("/GetById/{id}", GetStudentById).RequireAuthorization();
            studentGroup.MapGet("/GetByName/{name}", GetStudentByName).RequireAuthorization();
            studentGroup.MapPost("/Add", AddStudent).RequireAuthorization();
            studentGroup.MapPut("/Update/{id}", UpdateStudent).RequireAuthorization();
            studentGroup.MapDelete("/Delete/{id}", DeleteStudent).RequireAuthorization();
            studentGroup.MapGet("/GetRelatedSubjects/{id}", GetRelatedSubjects).RequireAuthorization();

            subjectGroup.MapGet("/GetAll", GetAllSubjects).RequireAuthorization();
            subjectGroup.MapGet("/GetById/{id}", GetSubjectById).RequireAuthorization();
            subjectGroup.MapGet("/GetByName/{name}", GetSubjectByName).RequireAuthorization();
            subjectGroup.MapPost("/Add", AddSubject).RequireAuthorization();
            subjectGroup.MapPut("/Update/{id}", UpdateSubject).RequireAuthorization();
            subjectGroup.MapDelete("/Delete/{id}", DeleteSubject).RequireAuthorization();
            subjectGroup.MapGet("/GetRelatedStudents/{id}", GetRelatedStudents).RequireAuthorization();
            subjectGroup.MapGet("/GetUnRelatedStudents/{id}", GetUnRelatedStudents).RequireAuthorization();
            subjectGroup.MapPost("/assignStudent/{subjectId}/{studentId}", AssignStudent).RequireAuthorization();
            subjectGroup.MapDelete("/UnassignStudent/{subjectId}/{studentId}", UnassignStudent).RequireAuthorization();
            subjectGroup.MapGet("/CheckNameExists/{name}", CheckSubjectExist).RequireAuthorization();


            // Get all students
            static async Task<IResult> GetAllStudents(IGenericRepository<Student, Subject> repository)
            {
                try
                {
                    var students = await repository.GetAllAsync();
                    List<StudentDTO> studentDTOs = new List<StudentDTO>();

                    foreach (var student in students)
                    {
                        var dateOfBirth = string.Concat(student.dateOfBirth.Year, "-", student.dateOfBirth.Month, "-", student.dateOfBirth.Day);
                        var studentDTO = new StudentDTO(student.id, student.name, dateOfBirth, student.age, student.address);
                        studentDTOs.Add(studentDTO);
                    }

                    return Results.Ok(studentDTOs);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

            }

            // Get student by ID        
            static async Task<IResult> GetStudentById(string id, IGenericRepository<Student, Subject> repository)
            {
                try
                {


                    var student = await repository.GetByIdAsync(id);
                    if (student != null)
                    {
                        var dateOfBirth = string.Concat(student.dateOfBirth.Year, "-", student.dateOfBirth.Month, "-", student.dateOfBirth.Day);
                        var studentDTO = new StudentDTO(student.id, student.name, dateOfBirth, student.age, student.address);
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

            // Get student by name
            static async Task<IResult> GetStudentByName(string name, IGenericRepository<Student,Subject> repository)
            {
                try
                {
                    var student = await repository.GetByNameAsync(name);
                    if (student != null)
                    {
                        var dateOfBirth = string.Concat(student.dateOfBirth.Year, "-", student.dateOfBirth.Month, "-", student.dateOfBirth.Day);
                        var studentDTO = new StudentDTO(student.id, student.name, dateOfBirth, student.age, student.address);
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

            // Add a new student
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
                    return Results.Created("", new StudentDTO(student.id, student.name, studentDTO.dateOfBirth, student.age, student.address));
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

            }

            // Update an existing student
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
                        return Results.Ok(new StudentDTO(id, student.name, studentDTO.dateOfBirth, student.age, student.address));
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

            // Delete a student
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

            // Get related subjects for a student
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

            // Get all subjects
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

            // Get subject by ID
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

            // Get subject by name
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

            // Check the subject exist or not
            static async Task<IResult> CheckSubjectExist(string name, IGenericRepository<Subject, Student> repository)
            {
                try
                {
                    var subject = await repository.GetByNameAsync(name);
                    if (subject != null)
                    {
                        return Results.Ok(new { nameExist = true });
                    }
                    else
                    {
                        return Results.Ok(new {nameExist = false} );
                    }
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }

            // Add a new subject
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

            // Update an existing subject
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

            // Delete a subject
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

            // Get related students for a subject
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
                        var dateOfBirth = string.Concat(student.dateOfBirth.Year, "-", student.dateOfBirth.Month, "-", student.dateOfBirth.Day);
                        var studentDTO = new StudentDTO(student.id, student.name, dateOfBirth, student.age, student.address);
                        studentDTOs.Add(studentDTO);
                    }

                    return Results.Ok(studentDTOs);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }

            // Get unrelated students for a subject
            static async Task<IResult> GetUnRelatedStudents(string id, IGenericRepository<Subject, Student> subjectRepository, IGenericRepository<Student, Subject> studentRepository)
            {
                try
                {
                    var subject = await subjectRepository.GetByIdAsync(id);
                    if (subject == null)
                    {
                        return Results.NotFound("Subject not found.");
                    }
                    var allStudents = await studentRepository.GetAllAsync();
                    var relatedStudents = await subjectRepository.GetRelatedEntitiesAsync(subject);
                    var unRelatedStudents = allStudents.Where(s => !relatedStudents.Any(rs => rs.id == s.id)).ToList();
                    List<StudentDTO> studentDTOs = new List<StudentDTO>();
                    foreach (var student in unRelatedStudents)
                    {
                        var dateOfBirth = string.Concat(student.dateOfBirth.Year, "-", student.dateOfBirth.Month, "-", student.dateOfBirth.Day);
                        var studentDTO = new StudentDTO(student.id, student.name, dateOfBirth, student.age, student.address);
                        studentDTOs.Add(studentDTO);
                    }
                    return Results.Ok(studentDTOs);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }



            // Assign a student to a subject
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

            // Unassign a student from a subject
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
                    studentRepository.RemoveEntityFromRelatedEntity(student, subject);
                    subjectRepository.RemoveEntityFromRelatedEntity(subject, student);
                    return Results.Ok(new { message = "Student unassigned from subject successfully." });


                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }

        }

    }
}
