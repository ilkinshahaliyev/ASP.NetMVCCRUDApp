using CRUDApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDApplication.Data
{
    public class CRUDAppDbContext : DbContext
    {
        public CRUDAppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
