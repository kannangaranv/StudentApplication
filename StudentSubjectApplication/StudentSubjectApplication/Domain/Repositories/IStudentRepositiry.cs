using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentSubjectApplication.Domain.Entities;

namespace StudentSubjectApplication.Domain.Repositories
{
    public interface IStudentRepositiry
    {
        void AddStudent(Student student);
        Student GetStudentById(int id);
        List<Student> GetAllStudents();
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
    }
}
