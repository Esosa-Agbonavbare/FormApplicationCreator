using FormApplicationCreator.Domain.Entities;

namespace FormApplicationCreator.Persistence.Repositories.Interface
{
    public interface IResponseRepository
    {
        Task AddAsync(IEnumerable<Response> responses);
        Task<List<Response>> GetAllByCandidateIdAsync(string candidateId);
        Task UpdateByCandidateIdAsync(string candidateId, List<Response> updatedResponses);
    }
}
