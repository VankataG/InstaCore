using InstaCore.Core.Dtos.Likes;

namespace InstaCore.Core.Services.Contracts
{
    public interface ILikeService
    {
        public Task<LikeResponse> LikeAsync(Guid userId, Guid postId);

        public Task<LikeResponse> UnlikeAsync(Guid userId, Guid postId);
    }
}
