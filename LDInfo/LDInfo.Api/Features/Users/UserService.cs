using LDInfo.Api.Features.Users.Models;
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

        /// <summary>
        /// Get all users.
        /// Client has a possibility to filter them by date to date sorted by date ascending
        /// Sort them by email, firstName or lastName
        /// Pagination functionality
        /// </summary>
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

        /// <summary>
        /// Get first 10 users with the most working hours with same filters and sortings as AllAsync
        /// </summary>
        public async Task<ICollection<User>> Top10Async(
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var entities = await this.AllAsync(fromDate, toDate);

            return entities.OrderByDescending(x => x.TimeLogs.Sum(x => x.Hours)).Take(10).ToList();
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

        /// <summary>
        /// Return Current User with worked hours by email
        /// </summary>
        public async Task<TopUserDetails> ByEmailAndDate(
            string email,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var entity = await dbContext.Users
                .AsNoTracking()
                .Include(u => u.TimeLogs)
                .ThenInclude(t => t.Project)
                .SingleOrDefaultAsync(x => x.Email == email);

            if (entity == null)
            {
                return null;
            }

            var workedHours = entity.TimeLogs.Sum(x => x.Hours);

            if (fromDate.HasValue && toDate.HasValue)
            {
                workedHours = entity.TimeLogs.Where(t => t.Date >= fromDate && t.Date <= toDate).Sum(x => x.Hours);
            }
            else if (fromDate.HasValue && !toDate.HasValue)
            {
                workedHours = entity.TimeLogs.Where(t => t.Date >= fromDate).Sum(x => x.Hours);
            }
            else if (!fromDate.HasValue && toDate.HasValue)
            {
                workedHours = entity.TimeLogs.Where(t => t.Date <= toDate).Sum(x => x.Hours);
            }

            var currentUser = new TopUserDetails
            {
                Email = email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                WorkedHours = (float)Convert.ToDecimal(string.Format("{0:F2}", workedHours))
        };

            return currentUser;
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
