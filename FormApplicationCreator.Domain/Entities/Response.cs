namespace FormApplicationCreator.Domain.Entities
{
    public class Response : BaseEntity
    {
        public string QuestionId { get; set; }
        public string Answer { get; set; }
        public string CandidateId { get; set; }
    }
}
