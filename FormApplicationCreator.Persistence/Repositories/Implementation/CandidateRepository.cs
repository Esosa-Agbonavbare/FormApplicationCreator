using FormApplicationCreator.Domain.Entities;
using FormApplicationCreator.Persistence.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FormApplicationCreator.Persistence.Repositories.Implementation
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly MyDbContext _context;

        public CandidateRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Candidate candidate)
        {
            await _context.Candidates.AddAsync(candidate);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Candidate candidate)
        {
            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Candidate>> GetAllAsync()
        {
            return await _context.Candidates.ToListAsync();
        }

        public async Task<Candidate> GetByIdAsync(string id)
        {
            return await _context.Candidates.FindAsync(id);
        }

        public async Task UpdateAsync(Candidate candidate)
        {
            _context.Candidates.Update(candidate);
            await _context.SaveChangesAsync();
        }
    }
}
