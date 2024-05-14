using System.ComponentModel.DataAnnotations;

namespace FormApplicationCreator.Application.DTOs
{
    public class AddApplicationFormDto
    {
        [Required(ErrorMessage = "The title field is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Select an option.")]
        public bool ShowPhone { get; set; } = false;

        [Required(ErrorMessage = "Select an option.")]
        public bool ShowNationality { get; set; } = false;

        [Required(ErrorMessage = "Select an option.")]
        public bool ShowCurrentResidence { get; set; } = false;

        [Required(ErrorMessage = "Select an option.")]
        public bool ShowIdNumber { get; set; } = false;

        [Required(ErrorMessage = "Select an option.")]
        public bool ShowDateOfBirth { get; set; } = false;

        [Required(ErrorMessage = "Select an option.")]
        public bool ShowGender { get; set; } = false;

        public List<AddQuestionDto> Questions { get; set; }
    }
}
