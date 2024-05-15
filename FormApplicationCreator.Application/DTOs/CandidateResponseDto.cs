using FormApplicationCreator.Domain.Enums;

namespace FormApplicationCreator.Application.DTOs
{
    public class CandidateResponseDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public Nationality Nationality { get; set; }
        public string NationalityString => Nationality.ToString().Replace("_", " ");
        public Nationality CurrentResidence { get; set; }
        public string CurrentResidenceString => CurrentResidence.ToString().Replace("_", " ");
        public string IdentificationNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string GenderString => Gender.ToString().Replace("_", " ");
        public List<ResponseDto> Responses { get; set; }
    }
}
