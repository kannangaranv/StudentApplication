using Azure;
using StudentSubjectApplication.Domain.Entities;
using StudentSubjectApplication.Domain.Repositories;
using StudentSubjectApplication.Infrastructure.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentSubjectApplication.Presentation
{
    public class Application
    {

        private readonly IStudentRepository _studentRepo;
        private readonly ISubjectRepository _subjectRepo;

        //public Application(IStudentRepository studentRepository, ISubjectRepository subjectRepository)
        //{
        //    _studentRepository1= studentRepository;
        //    _subjectRepository1= subjectRepository;
        //}

        private readonly IGenericRepository<Student, Subject> _studentRepository;
        private readonly IGenericRepository<Subject, Student> _subjectRepository;
        public Application(IGenericRepository<Student, Subject> studentRepository, IGenericRepository<Subject, Student> subjectRepository, IStudentRepository studentRepo, ISubjectRepository subjectRepo)
        {
            _studentRepository = studentRepository;
            _subjectRepository = subjectRepository;
            _studentRepo = studentRepo;
            _subjectRepo = subjectRepo;
        }


        public void Run()
        {
            try
            {
                //Declare variables
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
                string subjectId = "";
                string studentId = "";
                List<Student> studentListOfSubject = new List<Student>();
                List<Subject> subjectListOfStudent = new List<Subject>();

                //Sample Data Upload
                //string[,] sampleStudents = new string[,] { { "John", "23", "2000-8-9", "America" }, { "Ken", "21", "2000-10-8", "Sri Lanka" }, { "Bob", "21", "2000-10-8", "Sri Lanka" }, { "Ron", "29", "2001-10-8", "India" }, { "Ann", "21", "2011-10-8", "Sri Lanka" } };
                //string[,] sampleSubjects = new string[,] { { "English" }, { "Sinhala" }, { "Tamil" }, { "History" }, { "IT" } };
                //for (int i = 0; i < 5; i++)
                //{
                //    int sampleAge = int.Parse(sampleStudents[i, 1]);
                //    DateOnly sampleDate = DateOnly.Parse(sampleStudents[i, 2]);
                //    _studentRepository.AddStudent(sampleStudents[i, 0], sampleAge, sampleDate, sampleStudents[i, 3]);
                //    _subjectRepository.AddSubject(sampleSubjects[i, 0]);
                //}

                do
                {
                    //Display the menu
                    Console.Clear();
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine("Welcome to the Student Subject Application!");
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine("Please select an option:");
                    Console.WriteLine("1. Add Student");
                    Console.WriteLine("2. Add Subject");
                    Console.WriteLine("3. View All Students");
                    Console.WriteLine("4. View All Subjects");
                    Console.WriteLine("5. Assign Students to Subject");
                    Console.WriteLine("6. List Students of Subject");
                    Console.WriteLine("7. List Subjects of Student");
                    Console.WriteLine("8. Update Student");
                    Console.WriteLine("9. Update Subject");
                    Console.WriteLine("10. Delete Student");
                    Console.WriteLine("11. Delete Subject");
                    Console.WriteLine("12. Remove Student from Subject");
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
                        // Add Student------------------------------------------------------------------------------------
                        case "1": 
                            string anotherStudent = "y";

                            while (anotherStudent == "y")
                            {
                                //Enter valid student name
                                do
                                {
                                    Console.WriteLine("Enter student name : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        studentName = readResult.Trim();
                                    else
                                        studentName = "";

                                } while (studentName == "");

                                //Enter valid student age
                                do
                                {
                                    Console.WriteLine("Enter student age : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        validity = int.TryParse(readResult, out studentAge);
                                    else
                                        validity = false;
                                } while (validity == false);

                                //Enter valid student date of birth
                                do
                                {
                                    Console.WriteLine("Enter student date of birth (yyyy-mm-dd) : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                    {
                                        validity = DateOnly.TryParse(readResult, out dateOfBirth);
                                        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                                        if (validity == true && dateOfBirth > today)
                                            validity = false;
                                    }
                                    else
                                        validity = false;
                                } while (validity == false);

                                //Enter valid student address
                                do
                                {
                                    Console.WriteLine("Enter student address : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        address = readResult.Trim();
                                    else
                                        address = "";
                                } while (address == "");

                                //List<Student> existingStudents = _studentRepository.GetAllStudents();
                                List<Student> existingStudents = _studentRepository.GetAll();
                                int studentIdSeed = existingStudents.Count();
                                if (studentIdSeed == 0)
                                {
                                    studentIdSeed = 0;
                                }
                                else
                                {
                                    Student lastStudent = existingStudents.OrderByDescending(s => s.id).FirstOrDefault();
                                    if (lastStudent != null)
                                    {
                                        studentIdSeed = int.Parse(lastStudent.id.Substring(2));
                                    }
                                }
                                studentId = "ST" + (studentIdSeed + 1);
                                student = new Student(studentId, studentName, studentAge, dateOfBirth, address);

                                //_studentRepository.AddStudent(student);
                                _studentRepository.Add(student);
                                Console.WriteLine("Student added successfully!");

                                //Ask if the user wants to enter another student
                                do
                                {
                                    Console.WriteLine("Do you want to enter another student (y/n)");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        anotherStudent = readResult.Trim().ToLower();
                                } while (anotherStudent != "y" && anotherStudent != "n");

                            }

                            Console.WriteLine("Press any key to continue...");
                            Console.ReadLine();
                            break;

                        // Add Subject------------------------------------------------------------------------------------
                        case "2":
                            string anotherSubject = "y";

                            while (anotherSubject == "y")
                            {
                                //Enter valid subject name
                                do
                                {
                                    Console.WriteLine("Enter Subject Name : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        subjectName = readResult.Trim();
                                    else
                                        subjectName = "";
                                } while (subjectName == "");

                                //subject = _subjectRepository.GetSubjectByName(subjectName);
                                subject = _subjectRepository.GetByName(subjectName);

                                //Check if the subject already exists and if not add it
                                if (subject == null)
                                {
                                    //List<Subject> existingSubjects = _subjectRepository.GetAllSubjects();
                                    List<Subject> existingSubjects = _subjectRepository.GetAll();
                                    int subjectIdSeed = existingSubjects.Count();
                                    if (subjectIdSeed == 0)
                                    {
                                        subjectIdSeed = 0;
                                    }
                                    else
                                    {
                                        Subject lastSubject = existingSubjects.OrderByDescending(s => s.id).FirstOrDefault();
                                        if (lastSubject != null)
                                        {
                                            subjectIdSeed = int.Parse(lastSubject.id.Substring(3));
                                        }
                                    }
                                    subjectId = "SUB" + (subjectIdSeed + 1);
                                    subject = new Subject(subjectId, subjectName);
                                    //_subjectRepository.AddSubject(subject);
                                    _subjectRepository.Add(subject);
                                    Console.WriteLine("Subject added successfully!");
                                }
                                else 
                                    Console.WriteLine($"{subjectName} has Aldready added.");

                                //Ask if the user wants to enter another subject
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

                        // View All Students--------------------------------------------------------------------------------          
                        case "3":
                            //List<Student> students = _studentRepository.GetAllStudents();
                            List<Student> students = _studentRepository.GetAll();
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

                        // View All Subjects--------------------------------------------------------------------------------
                        case "4":
                            //List<Subject> subjects = _subjectRepository.GetAllSubjects();
                            List<Subject> subjects = _subjectRepository.GetAll();
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

                        // Assign Students to Subject-----------------------------------------------------------------------
                        case "5":
                            string anotherStudentAssign = "y";
                            string anotherAssign = "y";
                            while (anotherAssign == "y")
                            {
                                Console.WriteLine("Select a subject using following details : ");
                                //Display all subjects in the application
                                //subjects = _subjectRepository.GetAllSubjects();
                                subjects = _subjectRepository.GetAll();
                                if (subjects.Count != 0)
                                {
                                    Console.WriteLine("ID\tName");
                                    foreach (var sub in subjects)
                                    {
                                        Console.WriteLine($"{sub.id}\t{sub.name}");
                                    }

                                    //Select a subject with valid ID
                                    do
                                    {
                                        Console.WriteLine("Enter the subject ID : ");
                                        readResult = Console.ReadLine();
                                        if (readResult != null)
                                        {
                                            subjectId = readResult.ToUpper().Trim();
                                            //subject = _subjectRepository.GetSubjectById(subjectId);
                                            subject = _subjectRepository.GetById(subjectId);
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

                                    //Select students and assign to the subject
                                    //studentListOfSubject = _subjectRepository.GetStudentsOfSubject(subject);
                                    studentListOfSubject = _subjectRepository.GetRelatedEntities(subject);

                                    //students = _studentRepository.GetAllStudents();
                                    students = _studentRepository.GetAll();
                                    if (students.Count != 0)
                                    {
                                        //Check if all students are already assigned to the subject
                                        if (students.Count == studentListOfSubject.Count)
                                            Console.WriteLine($"All students have assigned to {subject.name}");
                                        else
                                        {
                                            Console.WriteLine("Select students using following details : ");
                                            //Display all students in the application
                                            Console.WriteLine("ID\t\tName\t\tAge\t\tDateOfBirth\t\tAddress");
                                            foreach (var std in students)
                                            {
                                                Console.WriteLine($"{std.id}\t\t{std.name}\t\t{std.age}\t\t{std.dateOfBirth}\t\t{std.address}");
                                            }

                                            while (anotherStudentAssign == "y")
                                            {
                                                //Select a student with valid ID
                                                do
                                                {
                                                    Console.WriteLine("Enter the Student ID : ");
                                                    readResult = Console.ReadLine();

                                                    if (readResult != null)
                                                    {
                                                        studentId = readResult.ToUpper().Trim();
                                                        //student = _studentRepository.GetStudentById(studentId);
                                                        student = _studentRepository.GetById(studentId);
                                                        if (student == null)
                                                        {
                                                            Console.WriteLine("Invalid Student ID.");
                                                            studentId = "";
                                                        }

                                                    }
                                                    else
                                                        studentId = "";

                                                } while (studentId == "");

                                                //Check if the student is already assigned to the subject, if not, assign the student to the subject
                                                if (studentListOfSubject.Contains(student))
                                                {
                                                    Console.WriteLine($"{student.name} is Already assigned to {subject.name}");
                                                }
                                                else
                                                {
                                                    studentListOfSubject.Add(student);
                                                    if (subject.relatedEntities == null)
                                                        subject.relatedEntities = new List<Student>();
                                                    (subject.relatedEntities).Add(student);
                                                    //_subjectRepository.UpdateSubject(subject);
                                                    _subjectRepository.Update(subject);
                                                    student.assigned = true;
                                                    if (student.relatedEntities == null)
                                                        student.relatedEntities = new List<Subject>();
                                                    (student.relatedEntities).Add(subject);          
                                                    //_studentRepository.UpdateStudent(student);
                                                    _studentRepository.Update(student);
                                                    Console.WriteLine($"{student.name} successfully assigned to {subject.name}");
                                                }

                                                //Check if all students are assigned to the subject
                                                if (students.Count == studentListOfSubject.Count)
                                                {
                                                    Console.WriteLine($"All students have assigned to {subject.name}");
                                                    break;
                                                }

                                                //Ask if the user wants to assign another student to the subject
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

                                //Ask if the user wants to assign students to another subject
                                
                                anotherStudentAssign = "y";
                                do
                                {
                                    Console.WriteLine("Do you want to do another assignment (y/n)");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        anotherAssign = readResult.ToLower().Trim();

                                } while (anotherAssign != "y" && anotherAssign != "n");

                            }

                            Console.WriteLine("Press any key to continue...");
                            Console.ReadLine();
                            break;

                        // List Students of Subject-----------------------------------------------------------------------
                        case "6":
                            string anotherSelection = "y";
                            Console.WriteLine("Choose the subject from following details");
                            //subjects = _subjectRepository.GetAllSubjects();
                            subjects = _subjectRepository.GetAll();
                            if (subjects.Count != 0)
                            {
                                //Display all subjects in the application
                                Console.WriteLine("ID\tName");
                                foreach (var sub in subjects)
                                {
                                    Console.WriteLine($"{sub.id}\t{sub.name}");
                                }
                                while (anotherSelection == "y")
                                {
                                    //Select a subject with valid ID
                                    do
                                    {
                                        Console.WriteLine("Enter the subject ID : ");
                                        readResult = Console.ReadLine();
                                        if (readResult != null)
                                        {
                                            subjectId = readResult.ToUpper().Trim();
                                            //subject = _subjectRepository.GetSubjectById(subjectId);
                                            subject = _subjectRepository.GetById(subjectId);
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

                                    //Get the list of students assigned to the subject and display them
                                    //studentListOfSubject = _subjectRepository.GetStudentsOfSubject(subject);
                                    studentListOfSubject = _subjectRepository.GetRelatedEntities(subject);
                                    if (studentListOfSubject.Count == 0)
                                        Console.WriteLine($"No students assigned to {subject.name}");
                                    else
                                    {
                                        Console.WriteLine("Assigned Students : ");
                                        foreach (var std in studentListOfSubject)
                                        {
                                            Console.WriteLine(std.name);
                                        }
                                    }

                                    //Ask if the user wants to get the details of students of another subject
                                    do
                                    {
                                        Console.WriteLine("Do you want to get the details of students of another subject (y/n) : ");
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

                        // List Subjects of Student-----------------------------------------------------------------------
                        case "7":
                            string anotherStudentSelection = "y";
                            Console.WriteLine("Choose the Student from following details");
                            //students = _studentRepository.GetAllStudents();
                            students = _studentRepository.GetAll();
                            if (students.Count != 0)
                            {
                                //Display all students in the application
                                Console.WriteLine("ID\tName");
                                foreach (var std in students)
                                {
                                    Console.WriteLine($"{std.id}\t{std.name}");
                                }
                                while (anotherStudentSelection == "y")
                                {
                                    //Select a student with valid ID
                                    do
                                    {
                                        Console.WriteLine("Enter the student ID : ");
                                        readResult = Console.ReadLine();
                                        if (readResult != null)
                                        {
                                            studentId = readResult.ToUpper().Trim();
                                            //student = _studentRepository.GetStudentById(studentId);
                                            student = _studentRepository.GetById(studentId);
                                            if (student == null)
                                            {
                                                Console.WriteLine("Invalid Student ID.");
                                                studentId = "";
                                            }

                                        }
                                        else
                                            studentId = "";
                                    } while (studentId == "");

                                    //Get the list of subjects assigned to the student and display them
                                    //subjectListOfStudent = _studentRepository.GetSubjectsofStudent(student);
                                    subjectListOfStudent = _studentRepository.GetRelatedEntities(student);
                                    if (subjectListOfStudent.Count != 0)
                                    {
                                        Console.WriteLine($"Subjects of {student.name} :");
                                        foreach (var sub in subjectListOfStudent)
                                        {
                                            Console.WriteLine(sub.name);
                                        }
                                    }
                                    else
                                        Console.WriteLine($"No subjects assigned for {student.name}");

                                    //Ask if the user wants to get the details of subjects of another student                          
                                    do
                                    {
                                        Console.WriteLine("Do you want to get the details of subjects of another student (y/n) : ");
                                        readResult = Console.ReadLine();
                                        if (readResult != null)
                                            anotherStudentSelection = readResult.ToLower().Trim();
                                    } while (anotherStudentSelection != "y" && anotherStudentSelection != "n");

                                }
                            }
                            else
                            {
                                Console.WriteLine("No students to select.");
                            }

                            Console.WriteLine("Press any key to continue...");
                            Console.ReadLine();
                            break;

                        // Update Student---------------------------------------------------------------------------------
                        case "8":
                            string anotherStudentUpdate = "y";
                            while (anotherStudentUpdate == "y")
                            {
                                //Select a student with valid ID
                                do
                                {
                                    Console.WriteLine("Enter the Student Id : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                    {
                                        studentId = readResult.ToUpper().Trim();
                                        //student = _studentRepository.GetStudentById(studentId);
                                        student = _studentRepository.GetById(studentId);
                                        if (student == null)
                                        {
                                            Console.WriteLine($"No student with id : {studentId}");
                                            studentId = "";
                                        }
                                    }
                                    else
                                        studentId = "";
                                } while (studentId == "");

                                //update the name with valid input
                                do
                                {
                                    Console.WriteLine($"Update student name ({student.name}) : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        studentName = readResult.Trim();
                                    else
                                        studentName = "";

                                } while (studentName == "");

                                //update the age with valid input
                                do
                                {
                                    Console.WriteLine($"Update student age ({student.age}) : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        validity = int.TryParse(readResult, out studentAge);
                                    else
                                        validity = false;
                                } while (validity == false);

                                //update the date of birth with valid input
                                do
                                {
                                    Console.WriteLine($"Update student date of birth (yyyy-mm-dd) ({student.dateOfBirth}) : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                    {
                                        validity = DateOnly.TryParse(readResult, out dateOfBirth);
                                        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                                        if (validity == true && dateOfBirth > today)
                                            validity = false;
                                    }
                                    else
                                        validity = false;
                                } while (validity == false);

                                //update the address with valid input
                                do
                                {
                                    Console.WriteLine($"Update student address ({student.address}) : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        address = readResult.Trim();
                                    else
                                        address = "";
                                } while (address == "");

                                student.name = studentName;
                                student.age = studentAge;
                                student.dateOfBirth = dateOfBirth;
                                student.address = address;
                                //_studentRepository.UpdateStudent(student);
                                _studentRepository.Update(student);
                                Console.WriteLine("Successfully Updated!");

                                //Ask if the user wants to update another student                               
                                do
                                {
                                    Console.WriteLine("Do you want to update another student (y/n) : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        anotherStudentUpdate = readResult.ToLower().Trim();

                                } while (anotherStudentUpdate != "y" && anotherStudentUpdate != "n");
                            }
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadLine();
                            break;

                        // Update Subject---------------------------------------------------------------------------------
                        case "9":
                            string anotherSubjectUpdate = "y";
                            while (anotherSubjectUpdate == "y")
                            {
                                //Select a subject with valid ID
                                do
                                {
                                    Console.WriteLine("Enter the Subject Id : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                    {
                                        subjectId = readResult.ToUpper().Trim();
                                        //subject = _subjectRepository.GetSubjectById(subjectId);
                                        subject = _subjectRepository.GetById(subjectId);
                                        if (subject == null)
                                        {
                                            Console.WriteLine($"No subject with id : {subjectId}");
                                            subjectId = "";
                                        }
                                    }
                                    else
                                        subjectId = "";
                                } while (subjectId == "");

                                //update the subject name with valid input
                                do
                                {
                                    Console.WriteLine($"Update Subject Name ({subject.name}) : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        subjectName = readResult.Trim();
                                    else
                                        subjectName = "";
                                } while (subjectName == "");

                                subject.name = subjectName;
                                //_subjectRepository.UpdateSubject(subject);
                                _subjectRepository.Update(subject);
                                Console.WriteLine("Successfully Updated!");

                                //Ask if the user wants to update another subject
                                do
                                {
                                    Console.WriteLine("Do you want to update another subject (y/n) : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        anotherSubjectUpdate = readResult.ToLower().Trim();
                                } while (anotherSubjectUpdate != "y" && anotherSubjectUpdate != "n");
                            }
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadLine();
                            break;

                        // Delete Student---------------------------------------------------------------------------------
                        case "10":
                            string anotherStudentDelete = "y";
                            while (anotherStudentDelete == "y")
                            {
                                //Select a student with valid ID
                                do
                                {
                                    Console.WriteLine("Enter the Student Id : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                    {
                                        studentId = readResult.ToUpper().Trim();
                                        //student = _studentRepository.GetStudentById(studentId);
                                        student = _studentRepository.GetById(studentId);
                                        if (student == null)
                                        {
                                            Console.WriteLine($"No student with id : {studentId}");
                                            studentId = "";
                                        }
                                    }
                                    else
                                        studentId = "";
                                } while (studentId == "");

                                //Check if the student is assigned to any subjects, if assigned, display the subjects
                                if (student.assigned == true)
                                {
                                    Console.WriteLine($"{student.name} has assigned to following subjects : ");
                                    //subjectListOfStudent = _studentRepository.GetSubjectsofStudent(student);
                                    subjectListOfStudent = _studentRepository.GetRelatedEntities(student);
                                    foreach (var sub in subjectListOfStudent)
                                    {
                                        Console.WriteLine(sub.name);
                                    }
                                    Console.WriteLine($"First remove {student.name} from above subjects.");
                                }
                                else
                                // If the student is not assigned to any subjects, ask for confirmation to delete and delete the student
                                {
                                    Console.WriteLine($"Do you want to delete {student.name} (y/n) : ");
                                    string confirmDelete = "n";
                                    do
                                    {
                                        readResult = Console.ReadLine();
                                        if (readResult != null)
                                            confirmDelete = readResult.ToLower().Trim();
                                    } while (confirmDelete != "y" && confirmDelete != "n");

                                    if (confirmDelete == "y")
                                    {
                                        //_studentRepository.DeleteStudent(student);
                                        _studentRepository.Delete(student);
                                        Console.WriteLine("Successfully Delected!");
                                    }
                                }

                                //Ask if the user wants to delete another student                                
                                do
                                {
                                    Console.WriteLine("Do you want to remove another student (y/n) : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        anotherStudentDelete = readResult.ToLower().Trim();

                                } while (anotherStudentDelete != "y" && anotherStudentDelete != "n");
                            }
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadLine();
                            break;

                        // Delete Subject---------------------------------------------------------------------------------
                        case "11":
                            string anotherSubjectDelete = "y";
                            while (anotherSubjectDelete == "y")
                            {
                                //Select a subject with valid ID
                                do
                                {
                                    Console.WriteLine("Enter the Subject Id : ");
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                    {
                                        subjectId = readResult.ToUpper().Trim();
                                        //subject = _subjectRepository.GetSubjectById(subjectId);
                                        subject = _subjectRepository.GetById(subjectId);
                                        if (subject == null)
                                        {
                                            Console.WriteLine($"No subject with id : {subjectId}");
                                            subjectId = "";
                                        }
                                    }
                                    else
                                        subjectId = "";
                                } while (subjectId == "");

                                Console.WriteLine($"Do you want to delete {subject.name} (y/n) : ");
                                string confirmDelete = "n";
                                do
                                {
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        confirmDelete = readResult.ToLower().Trim();
                                } while (confirmDelete != "y" && confirmDelete != "n");

                                if (confirmDelete == "y")
                                {
                                    //Check if the subject is assigned to any students, if assigned, remove the subject from the students' lists
                                    //_subjectRepository.DeleteSubjectsFromStudentLists(subject);
                                    //studentListOfSubject = _subjectRepository.GetStudentsOfSubject(subject);
                                    studentListOfSubject = _subjectRepository.GetRelatedEntities(subject);

                                    //foreach (var std in studentListOfSubject)
                                    //{
                                    //    //subjectListOfStudent = _studentRepository.GetSubjectsofStudent(std);
                                    //    subjectListOfStudent = _studentRepository.GetRelatedEntities(std);
                                    //    //_subjectRepository.RemoveStudentFromSubject(subject, std);
                                    //    _subjectRepository.RemoveRelatedEntity(subject, std);
                                    //    subjectListOfStudent.Remove(subject);
                                    //    if(subjectListOfStudent.Count == 0)
                                    //    {
                                    //        std.assigned = false;
                                    //        //_studentRepository.UpdateStudent(std);
                                    //        _studentRepository.Update(std);
                                    //    }
                                                           
                                    //}

                                    //_subjectRepository.DeleteSubject(subject);
                                    _subjectRepository.Delete(subject);
                                    Console.WriteLine("Successfully Deleted!");
                                }

                                //Ask if the user wants to delete another subject
                                Console.WriteLine("Do you want to remove another subject (y/n) : ");
                                readResult = Console.ReadLine();
                                do { 
                                    if (readResult != null)
                                        anotherSubjectDelete = readResult.ToLower().Trim();

                                } while (anotherSubjectDelete != "y" && anotherSubjectDelete != "n");
                            }
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadLine();
                            break;

                        // Unassign Student from Subject---------------------------------------------------------------
                        case "12":
                            string anotherStudentUnAssign = "y";
                            string anotherUnAssign = "y";
                            while (anotherUnAssign == "y")
                            {
                                Console.WriteLine("Select the subject that student needed to remove using following details : ");
                                //subjects = _subjectRepository.GetAllSubjects();
                                subjects = _subjectRepository.GetAll();
                                if (subjects.Count != 0)
                                {
                                    //Display all subjects in the application
                                    Console.WriteLine("ID\tName");
                                    foreach (var sub in subjects)
                                    {
                                        Console.WriteLine($"{sub.id}\t{sub.name}");
                                    }

                                    //Select a subject with valid ID
                                    do
                                    {
                                        Console.WriteLine("Enter the subject ID : ");
                                        readResult = Console.ReadLine();
                                        if (readResult != null)
                                        {
                                            subjectId = readResult.ToUpper().Trim();
                                            //subject = _subjectRepository.GetSubjectById(subjectId);
                                            subject = _subjectRepository.GetById(subjectId);
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

                                    //studentListOfSubject = _subjectRepository.GetStudentsOfSubject(subject);
                                    studentListOfSubject = _subjectRepository.GetRelatedEntities(subject);

                                    if (studentListOfSubject.Count != 0)
                                    {
                                        Console.WriteLine("Select students that needed to remove using following details : ");
                                        //Display all students assigned to the subject
                                        Console.WriteLine("ID\t\tName\t\tAge\t\tDateOfBirth\t\tAddress");
                                        foreach (var std in studentListOfSubject)
                                        {
                                            Console.WriteLine($"{std.id}\t\t{std.name}\t\t{std.age}\t\t{std.dateOfBirth}\t\t{std.address}");
                                        }

                                        while (anotherStudentUnAssign == "y")
                                        {
                                            //Select a student with valid ID
                                            do
                                            {
                                                Console.WriteLine("Enter the Student ID : ");
                                                readResult = Console.ReadLine();

                                                if (readResult != null)
                                                {
                                                    studentId = readResult.ToUpper().Trim();
                                                    //student = _studentRepository.GetStudentById(studentId);
                                                    student = _studentRepository.GetById(studentId);
                                                    if (student == null)
                                                    {
                                                        Console.WriteLine("Invalid Student ID.");
                                                        studentId = "";
                                                    }

                                                }
                                                else
                                                    studentId = "";

                                            } while (studentId == "");
                                            // Check if the student is assigned to the subject and remove the student from the subject
                                            if (studentListOfSubject.Contains(student))
                                            {
                                                //subjectListOfStudent = _studentRepository.GetRelatedEntities(student);
                                                _subjectRepo.RemoveStudentFromSubject(subject, student);
                                                _subjectRepository.RemoveRelatedEntity(subject, student);
                                                //subjectListOfStudent = _studentRepository.GetSubjectsofStudent(student);

                                                _studentRepo.RemoveSubjectFromStudent(student, subject);
                                                //_studentRepository.RemoveRelatedEntity(student, subject);
                                                subjectListOfStudent.Remove(subject);
                                                if (subjectListOfStudent.Count == 0)
                                                {
                                                    student.assigned = false;
                                                    //_studentRepository.UpdateStudent(student);
                                                    _studentRepository.Update(student);
                                                }
                             
                                                Console.WriteLine($"{student.name} successfully removed from {subject.name}");
                                            }
                                            else
                                            {
                                                Console.WriteLine($"{student.name} is not assigned to {subject.name}");
                                            }

                                            //if the student coun of the subject is zero, display a message and break the loop
                                            if (studentListOfSubject.Count == 0)
                                            {
                                                Console.WriteLine($"All students have removed from {subject.name}");
                                                break;
                                            }

                                            //Ask if the user wants to remove another student from the subject
                                            do
                                            {
                                                Console.WriteLine($"Do you want to remove an another student from {subject.name} (y/n)");
                                                readResult = Console.ReadLine();
                                                if (readResult != null)
                                                    anotherStudentUnAssign = readResult.ToLower().Trim();

                                            } while (anotherStudentUnAssign != "y" && anotherStudentUnAssign != "n");

                                        }
                                    }
                                    else
                                        Console.WriteLine("No students to remove.");
                                }
                                else
                                    Console.WriteLine("No subjects to select");

                                //Ask if the user wants to remove students from another subject
                                Console.WriteLine("Do you want to remove students from another subject (y/n)");
                                anotherStudentUnAssign = "y";
                                do
                                {
                                    readResult = Console.ReadLine();
                                    if (readResult != null)
                                        anotherUnAssign = readResult.ToLower().Trim();

                                } while (anotherUnAssign != "y" && anotherUnAssign != "n");

                            }

                            Console.WriteLine("Press any key to continue...");
                            Console.ReadLine();
                            break;

                        // Exit the application-----------------------------------------------------------------------
                        case "exit":
                            break;

                        // Invalid selection------------------------------------------------------------------------
                        default:
                            Console.WriteLine("Invalid selection. Please try again.");
                            break;
                    }
                } while (menuSelection != "exit");

            }catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
            }
        }

    }
}
