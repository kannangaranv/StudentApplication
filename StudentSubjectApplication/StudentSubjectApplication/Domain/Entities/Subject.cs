using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSubjectApplication.Domain.Entities
{
    public class Subject
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Student> students = new List<Student>();

        public Subject(string id, string name)
        {
            this.id = id;
            this.name = name;
        }



    }
}
