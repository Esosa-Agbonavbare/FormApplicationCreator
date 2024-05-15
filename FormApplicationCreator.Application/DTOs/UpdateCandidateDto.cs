using FormApplicationCreator.Domain.Enums;

namespace FormApplicationCreator.Application.DTOs
{
    public class UpdateCandidateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public Nationality Nationality { get; set; }
        public Nationality CurrentResidence { get; set; }
        public string IdentificationNumber { get; set; }
        public Gender Gender { get; set; }
        public List<AddResponseDto> Responses { get; set; }
    }
}
