using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentSubjectApplication.Domain.Entities;
using StudentSubjectApplication.Domain.Repositories;

namespace StudentSubjectApplication.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private List<Student> students = new List<Student>();

        public void AddStudent(string name, int age, DateOnly dateOfBirth, string address)
        {
            int studentCount = students.Count + 1;
            string id = "ST" + studentCount;
            Student student = new Student(id, name, age, dateOfBirth, address);
            students.Add(student);
        }

        public List<Student> GetAllStudents()
        {
            return students;
        }
    }

}
