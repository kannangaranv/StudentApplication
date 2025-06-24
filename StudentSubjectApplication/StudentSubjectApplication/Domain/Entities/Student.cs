using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSubjectApplication.Domain.Entities
{
    public class Student
    {
        public int id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public DateOnly dateOfBirth { get; set; }
        public string address { get; set; }

        public Student(int id, string name, int age, DateOnly dateOfBirth, string address)
        {
            this.id = id;
            this.name = name;
            this.age = age;
            this.dateOfBirth = dateOfBirth;
            this.address = address;
        }

    }
}
