using FormApplicationCreator.Domain.Entities;

namespace FormApplicationCreator.Persistence.Repositories.Interface
{
    public interface IApplicationFormRepository
    {
        Task AddAsync(ApplicationForm applicationForm);
        Task DeleteAsync(ApplicationForm applicationForm);
        Task<ApplicationForm> GetByIdAsync(string applicationFormId);
        Task<List<ApplicationForm>> GetAllAsync();
        Task UpdateAsync(ApplicationForm applicationForm);
    }
}
