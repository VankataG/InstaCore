using InstaCore.Core.Dtos.Posts;

namespace InstaCore.Core.Services.Contracts
{
    public interface IPostService
    {
        Task<PostResponse> CreateAsync(Guid userId, CreatePostRequest request);

        Task<PostResponse> GetByIdAsync(Guid postId);

        Task<IReadOnlyList<PostResponse>> GetByUserAsync(string username, int page, int pageSize);
    }
}
