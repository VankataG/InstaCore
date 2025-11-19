using InstaCore.Core.Models;

namespace InstaCore.Core.Contracts.Repos
{
    public interface IFollowRepository
    {
        Task<bool> ExistsAsync(Guid followerId, Guid followeeId);

        Task AddAsync(Follow follow);

        Task RemoveAsync(Guid followerId, Guid followeeId);

        Task<int> CountFollowerAsync(Guid userId);

        Task<int> CountFollowingAsync(Guid userId);

    }
}
