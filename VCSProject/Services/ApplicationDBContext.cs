using Microsoft.EntityFrameworkCore;
using VCSProject.Models;

namespace VCSProject.Services
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options) { 

        }

        public DbSet<Employee> Employees { get; set; }
    }
}
