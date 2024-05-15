using FormApplicationCreator.Domain.Enums;

namespace FormApplicationCreator.Application.DTOs
{
    public class QuestionResponseDto
    {
        public string QuestionId { get; set; }
        public QuestionType QuestionType { get; set; }
        public string QuestionTypeString => QuestionType.ToString().Replace("_", " ");
        public string QuestionText { get; set; }
        public List<string> Choices { get; set; }
        public int? MaxChoicesAllowed { get; set; }
        public bool EnableOtherOption { get; set; } = false;
    }
}
