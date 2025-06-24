using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSubjectApplication.Domain.Entities
{
    public class Subject
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Student> students = new List<Student>();

        public Subject(int id, string name)
        {
            this.id = id;
            this.name = name;
        }



    }
}
