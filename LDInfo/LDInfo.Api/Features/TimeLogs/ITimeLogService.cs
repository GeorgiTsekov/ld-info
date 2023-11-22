using LDInfo.Data.Entities;
using System.Globalization;

namespace LDInfo.Api.Features.TimeLogs
{
    public interface ITimeLogService
    {
        Task<TimeLog> ByIdAsync(int Id);
        Task<ICollection<TimeLog>> AllAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000);
        Task CreateAsync(TimeLog model);
        Task DeleteAll();
    }
}
