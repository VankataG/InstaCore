using InstaCore.Core.Contracts.Repos;
using InstaCore.Core.Dtos.Likes;
using InstaCore.Core.Models;

namespace InstaCore.Core.Mapping
{
    public static class LikeMapper
    {
        public static LikeResponse ToResponse(Like like, Guid postId, User user, int totalLikes) => new()
        {
            LikeId = like.Id,
            PostId = postId,
            Username = user.Username,
            Liked = true,
            TotalLikes = totalLikes,
            CreatedAt = like.CreatedAt
        };

        public static LikeResponse ToResponse(Guid postId, User user, int totalLikes) => new()
        {
            PostId = postId,
            Username = user.Username,
            Liked = true,
            TotalLikes = totalLikes,
        };
    }
}
