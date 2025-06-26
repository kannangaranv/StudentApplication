using StudentSubjectApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSubjectApplication.Domain.Repositories
{
    public interface ISubjectRepository
    {
        void AddSubject(string name);
        Subject GetSubjectById(string id);
        Subject GetSubjectByName(string name);
        List<Subject> GetAllSubjects();

        void UpdateSubject(Subject subject, string name);
        void DeleteSubject(Subject subject);

    }
}
