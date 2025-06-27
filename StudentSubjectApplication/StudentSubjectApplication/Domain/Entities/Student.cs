using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSubjectApplication.Domain.Entities
{
    public class Student
    {
        public string id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public DateOnly dateOfBirth { get; set; }
        public string address { get; set; }

        public bool assigned { get; set; }
        public virtual ICollection<Subject> AssignedSubjects { get; set; }
        public List<Subject> subjects = new List<Subject>();

        public Student(string id, string name, int age, DateOnly dateOfBirth, string address)
        {
            this.id = id;
            this.name = name;
            this.age = age;
            this.dateOfBirth = dateOfBirth;
            this.address = address;
        }

    }
}
