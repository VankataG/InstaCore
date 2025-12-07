using InstaCore.Core.Dtos.Comments;

namespace InstaCore.Core.Services.Contracts
{
    public interface ICommentService
    {
        public Task<CommentResponse> AddCommentAsync(Guid userId, Guid postId, CreateCommentRequest request);

        public Task<IEnumerable<CommentResponse>> ViewAllCommentsAsync(Guid postId);

        public Task DeleteCommentAsync(Guid userId, Guid commentId);
    }
}
