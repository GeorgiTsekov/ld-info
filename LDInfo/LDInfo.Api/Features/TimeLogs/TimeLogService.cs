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

        public async Task<ICollection<TimeLog>> AllAsync()
        {
            var entities = await dbContext.TimeLogs
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Project)
            .ToListAsync();

            return entities;
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
    }
}
