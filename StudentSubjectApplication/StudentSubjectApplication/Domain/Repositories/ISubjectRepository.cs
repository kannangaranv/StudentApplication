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
        List<Subject> GetAllSubjects();
        void UpdateSubject(string id, string name);
        void DeleteSubject(string id);

    }
}
