using LDInfo.Data;
using LDInfo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LDInfo.Api.Features.Users
{
    public class UserService : IUserService
    {
        private readonly LDInfoDbContext dbContext;

        public UserService(LDInfoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<User>> AllAsync()
        {
            var entities = await dbContext.Users
            .AsNoTracking()
            .Include(u => u.TimeLogs)
            .ThenInclude(t => t.Project)
            .ToListAsync();

            return entities;
        }

        public async Task<User> ByIdAsync(Guid Id)
        {
            var entity = await dbContext.Users
            .AsNoTracking()
            .Include(u => u.TimeLogs)
            .ThenInclude(t => t.Project)
            .SingleOrDefaultAsync(x => x.Id == Id);

            return entity;
        }

        public async Task<User> ByEmailAsync(string email)
        {
            var entity = await dbContext.Users
            .AsNoTracking()
            .Include(u => u.TimeLogs)
            .ThenInclude(t => t.Project)
            .SingleOrDefaultAsync(x => x.Email == email);

            return entity;
        }

        public async Task CreateAsync(User entity)
        {
            await dbContext.Users.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAll()
        {
            await dbContext.Users.ExecuteDeleteAsync();
            await dbContext.SaveChangesAsync();
        }
    }
}
