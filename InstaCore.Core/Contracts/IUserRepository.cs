using InstaCore.Core.Models;

namespace InstaCore.Core.Contracts
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByUsernameAsync(string username);

        Task<User?> GetByUsernameWithFollowsAsync(string username);

        Task<User?> GetByIdAsync(Guid id);

        Task<User?> GetByIdWithFollowsAsync(Guid id);

        Task<bool> ExistsByEmailAsync(string email);

        Task<bool> ExistsByUsernameAsync(string username);

        Task AddAsync(User user);

        Task UpdateAsync(User user);
    }
}
