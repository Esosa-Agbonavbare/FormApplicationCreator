using FormApplicationCreator.Domain.Entities;

namespace FormApplicationCreator.Persistence.Repositories.Interface
{
    public interface ICandidateRepository
    {
        Task AddAsync(Candidate candidate);
        Task<IEnumerable<Candidate>> GetAllAsync();
        Task<Candidate> GetByIdAsync(string id);
        Task UpdateAsync(Candidate candidate);
        Task DeleteAsync(Candidate candidate);
    }
}
