using FormApplicationCreator.Domain.Enums;

namespace FormApplicationCreator.Domain.Entities
{
    public class Candidate : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public Nationality Nationality { get; set; }
        public Nationality CurrentResidence { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public List<Response> Responses { get; set; }

        public ApplicationForm ApplicationForm { get; set; }
    }
}
