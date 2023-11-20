using LDInfo.Data.Entities;

namespace LDInfo.Api.Features.Users
{
    public interface IUserService
    {
        Task<User> ByIdAsync(Guid Id);
        Task<User> ByEmailAsync(string email);
        Task<ICollection<User>> AllAsync();
        Task CreateAsync(User model);
        Task DeleteAll();
    }
}
