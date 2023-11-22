using LDInfo.Data.Entities;

namespace LDInfo.Api.Features.Users
{
    public interface IUserService
    {
        Task<User> ByIdAsync(Guid Id);
        Task<User> ByEmailAsync(string email);
        Task<ICollection<User>> AllAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null, 
            string? sortBy = null, 
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000);
        Task<ICollection<User>> Top10Async(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? sortBy = null,
            bool isAscending = true);
        Task CreateAsync(User model);
        Task DeleteAll();
    }
}
