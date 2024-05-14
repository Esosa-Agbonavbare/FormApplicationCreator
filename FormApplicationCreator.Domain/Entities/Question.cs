using FormApplicationCreator.Domain.Enums;

namespace FormApplicationCreator.Domain.Entities
{
    public class Question : BaseEntity
    {
        public QuestionType QuestionType { get; set; }
        public string QuestionText { get; set; }
        public List<string> Choices { get; set; }
        public int? MaxChoicesAllowed { get; set; }
        public bool EnableOtherOption { get; set; }
        public string ApplicationFormId { get; set; }
    }
}
