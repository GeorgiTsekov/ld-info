using LDInfo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LDInfo.Data
{
    public class LDInfoDbContext : DbContext
    {
        public LDInfoDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<TimeLog> TimeLogs { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
