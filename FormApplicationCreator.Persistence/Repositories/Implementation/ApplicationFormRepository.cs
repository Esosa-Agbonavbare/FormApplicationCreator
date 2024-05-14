using FormApplicationCreator.Domain.Entities;
using FormApplicationCreator.Persistence.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FormApplicationCreator.Persistence.Repositories.Implementation
{
    public class ApplicationFormRepository : IApplicationFormRepository
    {
        private readonly MyDbContext _context;

        public ApplicationFormRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ApplicationForm applicationForm)
        {
            await _context.ApplicationForms.AddAsync(applicationForm);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ApplicationForm applicationForm)
        {
            _context.ApplicationForms.Remove(applicationForm);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ApplicationForm>> GetAllAsync()
        {
            return await _context.ApplicationForms.ToListAsync();
        }

        public async Task<ApplicationForm> GetByIdAsync(string applicationFormId)
        {
            return await _context.ApplicationForms.FindAsync(applicationFormId);
        }

        public async Task UpdateAsync(ApplicationForm applicationForm)
        {
            _context.ApplicationForms.Update(applicationForm);
            await _context.SaveChangesAsync();
        }
    }
}
