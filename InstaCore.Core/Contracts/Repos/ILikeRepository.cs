using InstaCore.Core.Models;

namespace InstaCore.Core.Contracts.Repos
{
    public interface ILikeRepository
    {
        public Task AddAsync(Like like);

        public Task DeleteAsync(Like like);

        public Task<Like?> GetByUserAndPostAsync(Guid userId, Guid postId);
    }
}
