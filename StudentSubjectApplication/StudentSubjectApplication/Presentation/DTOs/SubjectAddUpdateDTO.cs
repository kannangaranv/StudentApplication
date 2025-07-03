namespace StudentSubjectApplication.Presentation.DTOs
{
    public class SubjectAddUpdateDTO
    {
        public string name { get; set; }

        public SubjectAddUpdateDTO(string name)
        {
            this.name = name;
        }
    }
}
