using FormApplicationCreator.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FormApplicationCreator.Application.DTOs
{
    public class AddCandidateDto
    {
        [Required(ErrorMessage = "The first name field is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The last name field is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The email address field is required.")]
        [EmailAddress(ErrorMessage = "The email address is not a valid email address.")]
        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }
        public Nationality Nationality { get; set; }
        public Nationality CurrentResidence { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public List<AddResponseDto> Responses { get; set; }
    }
}
