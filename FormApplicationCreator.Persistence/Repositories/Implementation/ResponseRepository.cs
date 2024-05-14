using FormApplicationCreator.Domain.Entities;
using FormApplicationCreator.Persistence.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FormApplicationCreator.Persistence.Repositories.Implementation
{
    public class ResponseRepository : IResponseRepository
    {
        private readonly MyDbContext _context;

        public ResponseRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(IEnumerable<Response> responses)
        {
            _context.Responses.AddRange(responses);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Response>> GetAllByCandidateIdAsync(string candidateId)
        {
            return await _context.Responses
                    .Where(r => r.CandidateId == candidateId)
                    .ToListAsync();
        }

        public async Task UpdateByCandidateIdAsync(string candidateId, List<Response> updatedResponses)
        {
            var existingResponses = await _context.Responses.Where(r => r.CandidateId == candidateId).ToListAsync();
            foreach (var response in updatedResponses)
            {
                var existingResponse = existingResponses.FirstOrDefault(r => r.Id == response.Id);
                if (existingResponse != null)
                {
                    existingResponse.Answer = response.Answer;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
