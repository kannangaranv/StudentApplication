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
using System.ComponentModel.Design;

namespace StudentSubjectApplication.Presentation
{
    public class Application
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ISubjectRepository _subjectRepository;

        public Application(IStudentRepository studentRepository, ISubjectRepository subjectRepository)
        {
            _studentRepository = studentRepository;
            _subjectRepository = subjectRepository;
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
            Subject subject = null;
            Student student = null;
            List<Student> studentListOfSubject = new List<Student>();

            //Sample Data Upload
            string[,] sampleStudents = new string[,] { { "John", "23", "2000-8-9", "America" }, { "Ken", "21", "2000-10-8", "Sri Lanka" }, { "Bob", "21", "2000-10-8", "Sri Lanka" }, { "Ron", "29", "2001-10-8", "India" }, { "Ann", "21", "2011-10-8", "Sri Lanka" } };
            string[,] sampleSubjects = new string[,] { { "English" }, { "Sinhala" }, { "Tamil" }, { "History" }, { "IT" } };
            for (int i = 0; i < 5; i++)
            {
                int sampleAge = int.Parse(sampleStudents[i, 1]);
                DateOnly sampleDate = DateOnly.Parse(sampleStudents[i, 2]);
                _studentRepository.AddStudent(sampleStudents[i, 0], sampleAge, sampleDate, sampleStudents[i, 3]);
                _subjectRepository.AddSubject(sampleSubjects[i, 0]);
            }

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
                                Console.WriteLine("Enter student age : ");
                                readResult = Console.ReadLine();
                                if (readResult != null)
                                    validity = int.TryParse(readResult, out studentAge);
                                else
                                    validity = false;
                            } while (validity == false);

                            do
                            {
                                Console.WriteLine("Enter student date of birth (yyyy-mm-dd) : ");
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
                                Console.WriteLine("Enter student address : ");
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
                        string anotherSubject = "y";

                        while ( anotherSubject == "y")
                        {
                            do
                            {
                                Console.WriteLine("Enter Subject Name : ");
                                readResult = Console.ReadLine();
                                if (readResult != null)
                                    subjectName = readResult.Trim();
                                else
                                    subjectName = "";
                            } while (subjectName == "");

                            subject = _subjectRepository.GetSubjectByName(subjectName);

                            if (subject == null)
                            {
                                _subjectRepository.AddSubject(subjectName);
                                Console.WriteLine("Student added successfully!");
                            }
                            else
                                Console.WriteLine($"{subjectName} has Aldready added.");

                            do
                            {
                                Console.WriteLine("Do you want enter another subject (y/n)");
                                readResult = Console.ReadLine();
                                if (readResult != null)
                                    anotherSubject = readResult.Trim().ToLower();
                            } while (anotherSubject != "y" && anotherSubject != "n");
                            
                        }

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        break;
                    case "3":
                        List<Student> students = _studentRepository.GetAllStudents();
                        if (students.Count != 0)
                        {

                            Console.WriteLine("\t\t\t\t---------------------");
                            Console.WriteLine("\t\t\t\tStudents' Information");
                            Console.WriteLine("\t\t\t\t---------------------");
                            Console.WriteLine("ID\t\tName\t\tAge\t\tDateOfBirth\t\tAddress");
                            foreach (var std in students)
                            {
                                Console.WriteLine($"{std.id}\t\t{std.name}\t\t{std.age}\t\t{std.dateOfBirth}\t\t{std.address}");
                            }
                        }
                        else
                            Console.WriteLine("No Student Data");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        break;

                    case "4":
                        List<Subject> subjects = _subjectRepository.GetAllSubjects();
                        if (subjects.Count != 0)
                        {
                            Console.WriteLine("---------------------");
                            Console.WriteLine("Subjects' information");
                            Console.WriteLine("---------------------");
                            Console.WriteLine("ID\tName");
                            foreach (var sub in subjects)
                            {
                                Console.WriteLine($"{sub.id}\t{sub.name}");
                            }
                        }
                        else
                            Console.WriteLine("No Subject Data");
                        
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        break;

                    case "5":
                        string subjectId = "";
                        string studentId = "";
                        string anotherStudentAssign = "y";
                        string anotherAssign = "y";
                        while (anotherAssign == "y")
                        {
                            Console.WriteLine("Select a subject using following details : ");
                            subjects = _subjectRepository.GetAllSubjects();
                            if (subjects.Count != 0)
                            {
                                Console.WriteLine("ID\tName");
                                foreach (var sub in subjects)
                                {
                                    Console.WriteLine($"{sub.id}\t{sub.name}");
                                }
                                do
                                {
                                    Console.WriteLine("Enter the subject ID : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                    {
                                        subjectId = readResult.ToUpper().Trim();
                                        subject = _subjectRepository.GetSubjectById(subjectId);
                                        if (subject == null)
                                        {
                                            Console.WriteLine("Invalid Subject ID.");
                                            subjectId = "";
                                        }
                                    
                                    }

                                    else
                                        subjectId = "";
                                } while (subjectId == "");
                                Console.WriteLine($"You selected the {subject.name}");

                                studentListOfSubject = subject.students;

                                students = _studentRepository.GetAllStudents();
                                if (students.Count != 0)
                                {
                                    if (students.Count == studentListOfSubject.Count)
                                        Console.WriteLine($"All students have assigned to {subject.name}");
                                    else
                                    {
                                        Console.WriteLine("Select students using following details : ");
                                        Console.WriteLine("ID\t\tName\t\tAge\t\tDateOfBirth\t\tAddress");
                                        foreach (var std in students)
                                        {
                                            Console.WriteLine($"{std.id}\t\t{std.name}\t\t{std.age}\t\t{std.dateOfBirth}\t\t{std.address}");
                                        }

                                        while (anotherStudentAssign == "y")
                                        {
                                            do
                                            {
                                                Console.WriteLine("Enter the Student ID : ");
                                                readResult = Console.ReadLine();

                                                if (readResult != null)
                                                {
                                                    studentId = readResult.ToUpper().Trim();
                                                    student = _studentRepository.GetStudentById(studentId);
                                                    if (student == null)
                                                    {
                                                        Console.WriteLine("Invalid Student ID.");
                                                        studentId = "";
                                                    }
                                                    
                                                }
                                                else
                                                    studentId = "";

                                            } while (studentId == "");
                                            if (studentListOfSubject.Contains(student))
                                            {
                                                Console.WriteLine($"{student.name} is Already assigned to {subject.name}");
                                            }
                                            else
                                            {
                                                studentListOfSubject.Add(student);
                                                Console.WriteLine($"{student.name} successfully assigned to {subject.name}");
                                            }

                                            if (students.Count == studentListOfSubject.Count)
                                            {
                                                Console.WriteLine($"All students have assigned to {subject.name}");
                                                break;
                                            }

                                            Console.WriteLine($"Do you want to assign an another student to {subject.name} (y/n)");

                                            do
                                            {
                                                readResult = Console.ReadLine();
                                                if (readResult != null)
                                                    anotherStudentAssign = readResult.ToLower().Trim();

                                            } while (anotherStudentAssign != "y" && anotherStudentAssign != "n");

                                            
                                        }
                                    }
                                }
                                else
                                    Console.WriteLine("No students to select");
                            }
                            else
                                Console.WriteLine("No subjects to select");

                        Console.WriteLine("Do you want to do another assignment (y/n)");
                        anotherStudentAssign = "y";
                        do
                        {
                            readResult = Console.ReadLine();
                            if (readResult != null)
                                anotherAssign = readResult.ToLower().Trim();

                        } while (anotherAssign != "y" && anotherAssign != "n");

                        }

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        break;

                    case "6":
                        string anotherSelection = "y";
                        Console.WriteLine("Choose the subject from following details");
                        subjects = _subjectRepository.GetAllSubjects();
                        if (subjects.Count != 0)
                        {
                            Console.WriteLine("ID\tName");
                            foreach (var sub in subjects)
                            {
                                Console.WriteLine($"{sub.id}\t{sub.name}");
                            }
                            while (anotherSelection == "y")
                            {
                                do
                                {
                                    Console.WriteLine("Enter the subject ID : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                    {
                                        subjectId = readResult.ToUpper().Trim();
                                        subject = _subjectRepository.GetSubjectById(subjectId);
                                        if (subject == null)
                                        {
                                            Console.WriteLine("Invalid Subject ID.");
                                            subjectId = "";
                                        }

                                    }

                                    else
                                        subjectId = "";
                                } while (subjectId == "");
                                Console.WriteLine($"You selected the {subject.name}");

                                studentListOfSubject = subject.students;
                                if (studentListOfSubject.Count != 0)
                                {
                                    Console.WriteLine("Assigned Students : ");
                                    foreach (var std in studentListOfSubject)
                                    {
                                        Console.WriteLine(std.name);
                                    }
                                }
                                else
                                    Console.WriteLine($"No students assigned to {subject.name}");

                                Console.WriteLine("Do you want to get the details of students of another subject (y/n) : ");
                                do
                                {
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        anotherSelection = readResult.ToLower().Trim();
                                } while (anotherSelection != "y" && anotherSelection != "n");

                            }
                        }
                        else
                        {
                            Console.WriteLine("No subjects to select.");
                        }

                            Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        break;
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
