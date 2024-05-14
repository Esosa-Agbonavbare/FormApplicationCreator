namespace FormApplicationCreator.Domain.Entities
{
    public class ApplicationForm : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool ShowPhone { get; set; }
        public bool ShowNationality { get; set; }
        public bool ShowCurrentResidence { get; set; }
        public bool ShowIdNumber { get; set; }
        public bool ShowDateOfBirth { get; set; }
        public bool ShowGender { get; set; }
        public List<Question> Questions { get; set; }
    }
}
