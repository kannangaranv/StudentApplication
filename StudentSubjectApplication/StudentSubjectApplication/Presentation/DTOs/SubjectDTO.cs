using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace StudentSubjectApplication.Presentation.DTOs
{
    public class SubjectDTO
    {
        public string id { get; set; }
        public string name { get; set; }

        public SubjectDTO(string id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
