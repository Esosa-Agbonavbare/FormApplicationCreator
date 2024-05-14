using FormApplicationCreator.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FormApplicationCreator.Persistence
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<ApplicationForm> ApplicationForms { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Response> Responses { get; set; }
    }
}
