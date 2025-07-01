using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentSubjectApplication.Domain.Entities;
using StudentSubjectApplication.Domain.Repositories;
using StudentSubjectApplication.Infrastructure.DAL;


namespace StudentSubjectApplication.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {

        private readonly StudentContext studentContext;
        public StudentRepository(StudentContext studentContext)
        {
            this.studentContext = studentContext;
        }

        private static int studentIdSeed = 0;

        public void AddStudent(Student student)
        {
            studentContext.Students.Add(student);
            studentContext.SaveChanges();
        }

        public Student GetStudentById(string id)
        {
            Student student = studentContext.Students.FirstOrDefault(s => s.id == id);
            return student;
        }

        public List<Student> GetAllStudents()
        {
            return studentContext.Students.ToList();
        }

        public void UpdateStudent(Student student)
        {
            studentContext.Students.Update(student);
            studentContext.SaveChanges();
        }

        public void DeleteStudent(Student student)
        {
            studentContext.Students.Remove(student);
            studentContext.SaveChanges();
        }

        // Get all subjects of a student
        public List<Subject> GetSubjectsofStudent(Student student)
        {
            var subjectList = studentContext.Subjects
                .Where(s => s.relatedEntities.Any(st => st.id == student.id))
                .ToList();

            if (subjectList != null )
                return subjectList;
            else
                return new List<Subject>();

        }

        public void RemoveSubjectFromStudent(Student student, Subject subject)
        {
            var studentToUpdate = studentContext.Students
                .Include(s => s.relatedEntities)
                .FirstOrDefault(s => s.id == student.id);
            if (studentToUpdate != null && studentToUpdate.relatedEntities != null)
            {
                studentToUpdate.relatedEntities.Remove(subject);
                studentContext.SaveChanges();
            }
        }


    }

}
