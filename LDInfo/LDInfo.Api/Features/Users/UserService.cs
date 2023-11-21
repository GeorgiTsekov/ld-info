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

        public async Task<ICollection<User>> AllAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000)
        {
            var entities = dbContext.Users
                .AsNoTracking()
                .Include(u => u.TimeLogs)
                .ThenInclude(t => t.Project)
                .AsQueryable();

            // FIltering
            entities = FilterByDate(fromDate, toDate, entities);

            // Sorting
            entities = SortBy(sortBy, isAscending, entities);

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await entities.Skip(skipResults).Take(pageSize).ToListAsync();
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

        /// <summary>
        /// Filter all users from date to to date with ordered timelogs in ascending order
        /// </summary>
        private static IQueryable<User> FilterByDate(DateTime? fromDate, DateTime? toDate, IQueryable<User> entities)
        {
            if (fromDate.HasValue && toDate.HasValue)
            {
                entities = entities
                    .Where(u => u.TimeLogs.Any(t => t.Date >= fromDate && t.Date <= toDate))
                    .Include(u => u.TimeLogs.Where(t => t.Date >= fromDate && t.Date <= toDate).OrderBy(t => t.Date));
            }
            else if (fromDate.HasValue && !toDate.HasValue)
            {
                entities = entities
                    .Where(u => u.TimeLogs.Any(t => t.Date >= fromDate))
                    .Include(u => u.TimeLogs.Where(t => t.Date >= fromDate).OrderBy(t => t.Date));
            }
            else if (!fromDate.HasValue && toDate.HasValue)
            {
                entities = entities
                    .Where(u => u.TimeLogs.Any(t => t.Date <= toDate))
                    .Include(u => u.TimeLogs.Where(t => t.Date <= toDate).OrderBy(t => t.Date));
            }

            return entities;
        }

        /// <summary>
        /// Sort all users by firstName, lastname or email in descending or ascending order
        /// </summary>
        private static IQueryable<User> SortBy(string? sortBy, bool isAscending, IQueryable<User> entities)
        {
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.ToLower().Equals("firstName", StringComparison.OrdinalIgnoreCase))
                {
                    entities = isAscending
                        ? entities.OrderBy(x => x.FirstName)
                        : entities.OrderByDescending(x => x.FirstName);
                }
                else if (sortBy.ToLower().Equals("lastname", StringComparison.OrdinalIgnoreCase))
                {
                    entities = isAscending
                        ? entities.OrderBy(x => x.LastName)
                        : entities.OrderByDescending(x => x.LastName);
                }
                else if (sortBy.ToLower().Equals("email", StringComparison.OrdinalIgnoreCase))
                {
                    entities = isAscending
                        ? entities.OrderBy(x => x.Email)
                        : entities.OrderByDescending(x => x.Email);
                }
            }

            return entities;
        }
    }
}
