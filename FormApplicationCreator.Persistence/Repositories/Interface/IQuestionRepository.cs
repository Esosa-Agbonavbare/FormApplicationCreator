using FormApplicationCreator.Domain.Entities;
using FormApplicationCreator.Domain.Enums;

namespace FormApplicationCreator.Persistence.Repositories.Interface
{
    public interface IQuestionRepository
    {
        Task<Question> GetByIdAsync(string id);
        Task<List<Question>> GetAllByApplicationFormIdAsync(string applicationFormId);
        Task AddAsync(Question question);
        Task AddListAsync(IEnumerable<Question> questions);
        Task UpdateAsync(Question question);
        Task UpdateByApplicationFormIdAsync(string applicationFormId, List<Question> updatedQuestions);
        Task DeleteAsync(string id);
        Task<List<Question>> GetAllTypeByApplicationFormIdAsync(string applicationFormId, QuestionType type);
    }
}
