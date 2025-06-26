using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentSubjectApplication.Domain.Entities;
using StudentSubjectApplication.Domain.Repositories;

namespace StudentSubjectApplication.Infrastructure.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private List<Subject> allSubjects = new List<Subject>();

        private static int subjectIdSeed = 0;
        public void AddSubject(string name) {
            string subjectId = "SUB" + subjectIdSeed;
            Subject subject = new Subject(subjectId, name);
            allSubjects.Add(subject);
            subjectIdSeed++;
        }

        public Subject GetSubjectById(string id)
        {
            foreach (Subject subject in allSubjects) {
                if (subject.id == id)
                    return subject;
            }
            return null;
        }

        public Subject GetSubjectByName(string name)
        {
            foreach (Subject subject in allSubjects)
            {
                if ((subject.name).ToLower().Trim() == name.ToLower().Trim())
                    return subject;
            }
            return null;
        }

        public void UpdateSubject(Subject subject, string name)
        {
            subject.name = name;
        }

        public List<Subject> GetAllSubjects() { 
            return allSubjects; 
        }

        public void DeleteSubject(Subject subject)
        {
            allSubjects.Remove(subject);
        }
    }
}
