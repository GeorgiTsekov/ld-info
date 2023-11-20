using LDInfo.Data.Entities;
using LDInfo.Data;
using Microsoft.EntityFrameworkCore;

namespace LDInfo.Api.Features.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly LDInfoDbContext dbContext;

        public ProjectService(LDInfoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<Project>> AllAsync()
        {
            var entities = await dbContext.Projects
            .AsNoTracking()
            .Include(evt => evt.TimeLogs)
            .ToListAsync();

            return entities;
        }

        public async Task<Project> ByIdAsync(int Id)
        {
            var entity = await dbContext.Projects
            .AsNoTracking()
            .Include(u => u.TimeLogs)
            .SingleOrDefaultAsync(x => x.Id == Id);

            return entity;
        }

        public async Task<Project> ByNameAsync(string name)
        {
            var entity = await dbContext.Projects
            .AsNoTracking()
            .Include(u => u.TimeLogs)
            .SingleOrDefaultAsync(x => x.Name == name);

            return entity;
        }

        public async Task CreateAsync(Project entity)
        {
            await dbContext.Projects.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAll()
        {
            await dbContext.Projects.ExecuteDeleteAsync();
            await dbContext.SaveChangesAsync();
        }
    }
}
