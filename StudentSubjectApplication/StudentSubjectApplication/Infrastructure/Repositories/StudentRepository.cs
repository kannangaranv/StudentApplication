using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        //StudentContext studentContext = new StudentContext();

        private List<Student> students = new List<Student>();

        private static int studentIdSeed = 0;

        public void AddStudent(string name, int age, DateOnly dateOfBirth, string address)
        {


            string id = "ST" + studentIdSeed;
            Student student = new Student(id, name, age, dateOfBirth, address);
            students.Add(student);
            studentContext.Students.Add(student);
            studentContext.SaveChanges();
            studentIdSeed++;
        }

        public Student GetStudentById(string id)
        {
            foreach (Student student in students) { 
                if(student.id == id) 
                    return student;
            }
            return null;
        }

        public List<Student> GetAllStudents()
        {
            return studentContext.Students.ToList();
            //return students;
        }

        public void UpdateStudent(Student student,string name, int age, DateOnly dateOfBirth, string Address)
        {
            student.name = name;
            student.age = age;
            student.dateOfBirth = dateOfBirth;
            student.address = Address;
        }

        public void DeleteStudent(Student student)
        {
            students.Remove(student);
        }
    }

}
