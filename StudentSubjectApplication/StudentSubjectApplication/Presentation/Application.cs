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
            int studentAge = 0;
            DateOnly dateOfBirth = new DateOnly();
            string address = "";
            string subjectName = "";

            string? readResult;
            string menuSelection = "";
            bool validity = false;

            do
            { 
                Console.Clear();
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Welcome to the Student Subject Application!");
                Console.WriteLine("-------------------------------------------");
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
                        string anotherStudent = "y";

                        while (anotherStudent == "y")
                        {
                            do
                            {
                                Console.WriteLine("Enter student name : ");
                                readResult = Console.ReadLine();
                                if (readResult != null)
                                    studentName = readResult.Trim();
                                else
                                    studentName = "";

                            } while (studentName == "");

                            do
                            {
                                Console.WriteLine("Enter student age:");
                                readResult = Console.ReadLine();
                                if (readResult != null)
                                    validity = int.TryParse(readResult, out studentAge);
                                else
                                    validity = false;
                            } while (validity == false);

                            do
                            {
                                Console.WriteLine("Enter student date of birth (yyyy-mm-dd):");
                                readResult = Console.ReadLine();
                                if ( readResult != null)
                                {
                                    validity = DateOnly.TryParse(readResult, out dateOfBirth);
                                    DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                                    if (validity == true && dateOfBirth > today)
                                        validity = false;
                                }                                   
                                else 
                                    validity = false;
                            } while (validity == false);

                            do
                            {
                                Console.WriteLine("Enter student address:");
                                readResult = Console.ReadLine();
                                if (readResult != null)
                                    address = readResult.Trim();
                                else
                                    address = "";
                            } while (address == "");

                            _studentRepository.AddStudent(studentName, studentAge, dateOfBirth, address);
                            Console.WriteLine("Student added successfully!");

                            Console.WriteLine("Do you want to enter another student (y/n)");
                            do
                            {
                                readResult = Console.ReadLine();
                                if (readResult != null)
                                    anotherStudent = readResult.Trim().ToLower();
                            } while (anotherStudent != "y" && anotherStudent != "n");
                            
                        }
         
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        break;

                    case "2":
                        // Add subject logic here
                        break;
                    case "3":
                        List<Student> students = _studentRepository.GetAllStudents();
                        Console.WriteLine("\t\t\t\t---------------------");
                        Console.WriteLine("\t\t\t\tStudents' Information");
                        Console.WriteLine("\t\t\t\t---------------------");
                        foreach (var student in students)
                        {
                            Console.WriteLine("ID\t\tName\t\tAge\t\tDateOfBirth\t\tAddress");
                            Console.WriteLine($"{student.id}\t\t{student.name}\t\t{student.age}\t\t{student.dateOfBirth}\t\t{student.address}");
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
