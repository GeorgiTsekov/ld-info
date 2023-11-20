using LDInfo.Data.Entities;

namespace LDInfo.Api.Features.Projects
{
    public interface IProjectService
    {
        Task<Project> ByIdAsync(int Id);
        Task<Project> ByNameAsync(string name);
        Task<ICollection<Project>> AllAsync();
        Task CreateAsync(Project model);
        Task DeleteAll();
    }
}
