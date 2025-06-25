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
        public void AddSubject(string name) {
            int subjectCount = allSubjects.Count + 1;
            string subjectId = "SUB" + subjectCount;
            Subject subject = new Subject(subjectId, name);
            allSubjects.Add(subject);
        }

        public Subject GetSubjectById(string id)
        {
            foreach (Subject subject in allSubjects) {
                if (subject.id == id)
                    return subject;
            }
            return null;
        }
        
        public List<Subject> GetAllSubjects() { 
            return allSubjects; 
        }
    }
}
