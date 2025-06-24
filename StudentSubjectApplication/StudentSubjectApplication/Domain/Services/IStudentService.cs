using StudentSubjectApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSubjectApplication.Domain.Services
{
    public interface IStudentService
    {
        void AssignStudentToSubject(string studentId, string subjectId);
        void RemoveStudentFromSubject(string studentId, string subjectId);
        List<Subject> GetStudentsOfSubject(string subjectId);
    }
}
