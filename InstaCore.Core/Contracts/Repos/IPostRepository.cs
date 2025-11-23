using InstaCore.Core.Models;

namespace InstaCore.Core.Contracts.Repos
{
    public interface IPostRepository
    {
        Task AddAsync(Post post);

        Task<Post?> GetByIdAsync(Guid id);

        Task<IReadOnlyList<Post>> GetByUserAsync(Guid userId, int skip, int take);

        Task<IReadOnlyList<Post>> GetFeedAsync(IReadOnlyList<Guid> authorIds, int skip, int take);
    }
}
