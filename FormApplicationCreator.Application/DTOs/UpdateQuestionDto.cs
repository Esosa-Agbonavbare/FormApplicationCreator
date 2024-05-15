using FormApplicationCreator.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FormApplicationCreator.Application.DTOs
{
    public class UpdateQuestionDto
    {
        [Required(ErrorMessage = "The Question type field is required.")]
        public QuestionType QuestionType { get; set; }

        [Required(ErrorMessage = "The Question field is required.")]
        public string QuestionText { get; set; }

        public List<string> Choices { get; set; }
        public int? MaxChoicesAllowed { get; set; }
        public bool EnableOtherOption { get; set; } = false;
    }
}
