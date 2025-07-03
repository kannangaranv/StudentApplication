namespace StudentSubjectApplication.Presentation.DTOs
{
    public class StudentAddUpdateDTO
    {
        public string name { get; set; }
        public string dateOfBirth { get; set; }
        public int age { get; set; }
        public string address { get; set; }

        public StudentAddUpdateDTO(string name, string dateOfBirth, int age, string address)
        {
            this.name = name;
            this.dateOfBirth = dateOfBirth;
            this.age = age;
            this.address = address;
        }
    }
}
