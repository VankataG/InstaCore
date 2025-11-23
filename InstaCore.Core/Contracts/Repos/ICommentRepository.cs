using InstaCore.Core.Models;

namespace InstaCore.Core.Contracts.Repos
{
    public interface ICommentRepository
    {
        public Task<Comment?> GetByIdAsync(Guid id);

        public Task AddAsync(Comment comment);

        public Task DeleteAsync(Comment comment);

        public Task<IEnumerable<Comment>> GetAllCommentsAsync(Guid postId);
    }
}
