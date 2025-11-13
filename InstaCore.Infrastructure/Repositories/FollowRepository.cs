using InstaCore.Core.Contracts;
using InstaCore.Core.Models;

namespace InstaCore.Infrastructure.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        public Task AddAsync(Follow follow)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountFollowerAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountFollowingAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid followerId, Guid followeeId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Guid followerId, Guid followeeId)
        {
            throw new NotImplementedException();
        }
    }
}
