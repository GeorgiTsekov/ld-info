using LDInfo.Data.Entities;

namespace LDInfo.Api.Features.TimeLogs
{
    public interface ITimeLogService
    {
        Task<TimeLog> ByIdAsync(int Id);
        Task<ICollection<TimeLog>> AllAsync();
        Task CreateAsync(TimeLog model);
        Task DeleteAll();
    }
}
