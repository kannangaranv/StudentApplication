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
    public class SubjectRepository : ISubjectRepository
    {
        private readonly StudentContext studentContext;

        public SubjectRepository(StudentContext studentContext)
        {
            this.studentContext = studentContext;
        }

        private static int subjectIdSeed = 0;

        public void AddSubject(string name)
        {
            subjectIdSeed = studentContext.Subjects.Count();
            if(subjectIdSeed == 0) {
                subjectIdSeed = 0; 
            } else {
                Subject lastSubject = studentContext.Subjects.OrderByDescending(s => s.id).FirstOrDefault();
                if (lastSubject != null)
                {
                    subjectIdSeed = int.Parse(lastSubject.id.Substring(3)); 
                }
            }
            string subjectId = "SUB" + (subjectIdSeed + 1);
            Subject subject = new Subject(subjectId, name);
            studentContext.Subjects.Add(subject);
            studentContext.SaveChanges();
        }

        public Subject GetSubjectById(string id)
        {
            Subject subject = studentContext.Subjects.FirstOrDefault(s => s.id == id);
            return subject;
        }

        public Subject GetSubjectByName(string name)
        {
            Subject subject = studentContext.Subjects.FirstOrDefault(s => s.name.ToLower().Trim() == name.ToLower().Trim());
            return subject;
        }

        public void UpdateSubject(Subject subject)
        {
            studentContext.Subjects.Update(subject);
            studentContext.SaveChanges();
        }

        public List<Subject> GetAllSubjects()
        {
            return studentContext.Subjects.ToList();
        }

        public void DeleteSubject(Subject subject)
        {
            studentContext.Subjects.Remove(subject);
            studentContext.SaveChanges();
        }

        //Remove subject from all students' lists
        public void DeleteSubjectsFromStudentLists(Subject subject)
        {
            var students = studentContext.Students
                .Include(s => s.subjects)
                .Where(s => s.subjects.Any(sub => sub.id == subject.id))
                .ToList();

            foreach (var student in students)
            {
                student.subjects.Remove(subject);
                if (student.subjects.Count == 0)
                {
                    student.assigned = false;
                }
            }
            studentContext.SaveChanges();
        }

        // Get all students enrolled in a specific subject
        public List<Student> GetStudentsOfSubject(Subject subject)
        {
            var studentList = studentContext.Students
                .Where(s => s.subjects.Any(sub => sub.id == subject.id))
                .ToList();
            if (studentList != null)
                return studentList;
            else
                return new List<Student>();
        }

        // Remove a subject from a student's list and vice versa
        public void RemoveSubjectFromStudent(Subject subject, Student student)
        {
            var std = studentContext.Students
                .Include(s => s.subjects)
                .FirstOrDefault(s => s.id == student.id);
            if (std != null && std.subjects.Contains(subject))
            {
                std.subjects.Remove(subject);
                if (std.subjects.Count == 0)
                {
                    std.assigned = false;
                }
                studentContext.SaveChanges();
            }
                

            var sub = studentContext.Subjects
                .Include(s => s.students)
                .FirstOrDefault(s => s.id == subject.id);
            if (sub != null && sub.students.Contains(student))
            {
                sub.students.Remove(student);
                studentContext.SaveChanges();
            }
                


        }
    }

 }
