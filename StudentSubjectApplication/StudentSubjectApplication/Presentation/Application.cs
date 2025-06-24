using StudentSubjectApplication.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using StudentSubjectApplication.Domain.Entities;
using StudentSubjectApplication.Domain.Repositories;
using StudentSubjectApplication.Infrastructure.Repositories;

namespace StudentSubjectApplication.Presentation
{
    public class Application
    {
        private readonly IStudentRepository _studentRepository;

        public Application(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void Run()
        {
            string studentName = "";
            string age = "";
            string dateOfBirth = "";
            string address = "";
            string subjectName = "";

            string? readResult;
            string menuSelection = "";

            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Student Subject Application!");
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Add Subject");
                Console.WriteLine("3. View All Students");
                Console.WriteLine("4. View All Subjects");
                Console.WriteLine("5. Assign Student to Subject");
                Console.WriteLine("6. List Students of Subject");
                Console.WriteLine("7. Update Student");
                Console.WriteLine("8. Update Subject");
                Console.WriteLine("9. Delete Student");
                Console.WriteLine("10. Delete Subject");
                Console.WriteLine("11. Remove Student from Subject");
                Console.WriteLine("Type 'exit' to quit the application.");

                readResult = Console.ReadLine();

                if (readResult != null)
                {
                    readResult = readResult.Trim().ToLower();
                    menuSelection = readResult;
                }

                Console.WriteLine($"You selected the option {menuSelection}");

                switch (menuSelection)
                {
                    case "1":
                        do
                        {
                            Console.WriteLine("Enter student name : ");
                            readResult = Console.ReadLine();
                            if (readResult != null)
                                {
                                    studentName = readResult.Trim();
                                }
                            else
                                {
                                    studentName = "";
                                }

                        } while (studentName != "");

                        Console.WriteLine("Enter student name:");
                        studentName = Console.ReadLine();
                        Console.WriteLine("Enter student age:");
                        age = Console.ReadLine();
                        Console.WriteLine("Enter student date of birth (yyyy-mm-dd):");
                        dateOfBirth = Console.ReadLine();
                        Console.WriteLine("Enter student address:");
                        address = Console.ReadLine();
                        _studentRepository.AddStudent(studentName, age, dateOfBirth, address);
                        Console.WriteLine("Student added successfully!");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        break;
                    case "2":
                        // Add subject logic here
                        break;
                    case "3":
                        List<Student> students = _studentRepository.GetAllStudents();
                        foreach (var student in students)
                        {
                            Console.WriteLine($"ID: {student.id}, Name: {student.name}, Age: {student.age}, Date of Birth: {student.dateOfBirth}, Address: {student.address}");
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        break;
                    // Other cases for subjects and operations...
                    case "exit":
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }


            } while(menuSelection != "exit");

           

        }


    }
}
