using LDInfo.Data.Entities;
using LDInfo.Data;
using Microsoft.EntityFrameworkCore;

namespace LDInfo.Api.Features.TimeLogs
{
    public class TimeLogService : ITimeLogService
    {
        private readonly LDInfoDbContext dbContext;

        public TimeLogService(LDInfoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<TimeLog>> AllAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000)
        {
            var entities = dbContext.TimeLogs
                .AsNoTracking()
                .Include(u => u.User)
                .Include(t => t.Project)
                .AsQueryable();

            // FIltering
            entities = FilterByDate(fromDate, toDate, entities);

            // Sorting
            entities = SortBy(sortBy, isAscending, entities);

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await entities.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<TimeLog> ByIdAsync(int Id)
        {
            var entity = await dbContext.TimeLogs
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Project)
            .SingleOrDefaultAsync(x => x.Id == Id);

            return entity;
        }

        public async Task CreateAsync(TimeLog entity)
        {
            await dbContext.TimeLogs.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAll()
        {
            await dbContext.TimeLogs.ExecuteDeleteAsync();
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Filter all users from date to to date with ordered timelogs in ascending order
        /// </summary>
        private static IQueryable<TimeLog> FilterByDate(DateTime? fromDate, DateTime? toDate, IQueryable<TimeLog> entities)
        {
            if (fromDate.HasValue && toDate.HasValue)
            {
                entities = entities.Where(t => t.Date >= fromDate && t.Date <= toDate);
            }
            else if (fromDate.HasValue && !toDate.HasValue)
            {
                entities = entities = entities.Where(t => t.Date >= fromDate);
            }
            else if (!fromDate.HasValue && toDate.HasValue)
            {
                entities = entities = entities.Where(t => t.Date <= fromDate);
            }

            return entities;
        }

        /// <summary>
        /// Sort all users by firstName, lastname or email in descending or ascending order
        /// </summary>
        private static IQueryable<TimeLog> SortBy(string? sortBy, bool isAscending, IQueryable<TimeLog> entities)
        {
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.ToLower().Equals("date", StringComparison.OrdinalIgnoreCase))
                {
                    entities = isAscending
                        ? entities.OrderBy(x => x.Date)
                        : entities.OrderByDescending(x => x.Date);
                }
                else if (sortBy.ToLower().Equals("hours", StringComparison.OrdinalIgnoreCase))
                {
                    entities = isAscending
                        ? entities.OrderBy(x => x.Hours)
                        : entities.OrderByDescending(x => x.Hours);
                }
                else if (sortBy.ToLower().Equals("project", StringComparison.OrdinalIgnoreCase))
                {
                    entities = isAscending
                        ? entities.OrderBy(x => x.Project.Name)
                        : entities.OrderByDescending(x => x.Project.Name);
                }
                else if (sortBy.ToLower().Equals("email", StringComparison.OrdinalIgnoreCase))
                {
                    entities = isAscending
                        ? entities.OrderBy(x => x.User.Email)
                        : entities.OrderByDescending(x => x.User.Email);
                }
            }

            return entities;
        }
    }
}
