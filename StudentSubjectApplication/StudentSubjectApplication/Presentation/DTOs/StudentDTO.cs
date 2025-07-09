namespace StudentSubjectApplication.Presentation.DTOs
{
    public class StudentDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public string dateOfBirth { get; set; }
        public int age { get; set; }
        public string address { get; set; }

        public StudentDTO(string id, string name, String dateOfBirth, int age, string address)
        {
            this.id = id;
            this.name = name;
            this.dateOfBirth = dateOfBirth;
            this.age = age;
            this.address = address;
        }
    }
}
