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
        void AddStudent(string name, int age, DateOnly dateOfBirth, string Address);
        Student GetStudentById(string id);
        List<Student> GetAllStudents();
        void UpdateStudent(Student student);
        void DeleteStudent(Student student);
        List<Subject> GetSubjectsofStudent(Student sudent);

        
    }
}
