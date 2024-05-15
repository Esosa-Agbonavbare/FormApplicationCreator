using FormApplicationCreator.Domain.Entities;
using FormApplicationCreator.Domain.Enums;
using FormApplicationCreator.Persistence.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FormApplicationCreator.Persistence.Repositories.Implementation
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly MyDbContext _context;

        public QuestionRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
        }

        public async Task AddListAsync(IEnumerable<Question> questions)
        {
            _context.Questions.AddRange(questions);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var question = await GetByIdAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Question>> GetAllByApplicationFormIdAsync(string applicationFormId)
        {
            return await _context.Questions
                    .Where(r => r.ApplicationFormId == applicationFormId)
                    .ToListAsync();
        }

        public async Task<Question> GetByIdAsync(string id) =>
            await _context.Questions.FindAsync(id);

        public async Task<List<Question>> GetAllTypeByApplicationFormIdAsync(string applicationFormId, QuestionType type)
        {
            return await _context.Questions
                .Where(q => q.ApplicationFormId == applicationFormId && q.QuestionType == type)
                .ToListAsync();
        }

        public async Task UpdateAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateByApplicationFormIdAsync(string applicationFormId, List<Question> updatedQuestions)
        {
            var existingQuestions = await _context.Questions.Where(r => r.ApplicationFormId == applicationFormId).ToListAsync();
            foreach (var response in updatedQuestions)
            {
                var existingQuestion = existingQuestions.FirstOrDefault(r => r.Id == response.Id);
                if (existingQuestion != null)
                {
                    existingQuestion.QuestionText = response.QuestionText;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
