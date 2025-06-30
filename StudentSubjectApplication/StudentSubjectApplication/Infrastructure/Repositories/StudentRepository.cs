using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentSubjectApplication.DAL;
using StudentSubjectApplication.Domain.Entities;
using StudentSubjectApplication.Domain.Repositories;


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

        public void AddStudent(string name, int age, DateOnly dateOfBirth, string address)
        {
            studentIdSeed = studentContext.Students.Count();
            if (studentIdSeed == 0)
            {
                studentIdSeed = 0; 
            } else
            {
                Student lastStudent = studentContext.Students.OrderByDescending(s => s.id).FirstOrDefault();
                if (lastStudent != null)
                {
                    studentIdSeed = int.Parse(lastStudent.id.Substring(2)); 
                }
            }
            string id = "ST" + (studentIdSeed + 1);
            Student student = new Student(id, name, age, dateOfBirth, address);
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
                .Where(s => s.students.Any(st => st.id == student.id))
                .ToList();

            if (subjectList != null )
                return subjectList;
            else
                return new List<Subject>();

        }
    }

}
