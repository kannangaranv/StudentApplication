using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentSubjectApplication.Domain.Entities;

namespace StudentSubjectApplication.Domain.Repositories
{
    public interface IStudentRepository
    {
        public void AddStudent(string name, int age, DateOnly dateOfBirth, string Address);
        //Student GetStudentById(string id);
        public List<Student> GetAllStudents();
        //void UpdateStudent(string id, string name, int age, DateOnly dateOfBirth, string Address);
        //void DeleteStudent(string id);
    }
}
