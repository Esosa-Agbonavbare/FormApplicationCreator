using System.ComponentModel.DataAnnotations;

namespace FormApplicationCreator.Application.DTOs
{
    public class AddResponseDto
    {
        [Required(ErrorMessage = "The answer field is required.")]
        public string Answer { get; set; }
    }
}
