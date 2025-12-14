using InstaCore.Core.Dtos.Posts;
using InstaCore.Core.Models;

namespace InstaCore.Core.Mapping
{
    public static class PostMapper
    {
        public static PostResponse ToResponse(Post post, User user) => new()
        {
            Id = post.Id,
            Username = user.Username,
            UserAvatarUrl = user.AvatarUrl,
            Caption = post.Caption,
            ImageUrl = post.ImageUrl,
            Likes = post.Likes.Count,
            Comments = post.Comments.Count,
            CreatedAt = post.CreatedAt
        };

        public static PostResponse ToResponse(Post post, Guid? currentUserId = null) => new()
        {
            Id = post.Id,
            Username = post.User.Username,
            UserAvatarUrl = post.User.AvatarUrl,
            Caption = post.Caption,
            ImageUrl = post.ImageUrl,
            Likes = post.Likes.Count,
            Comments = post.Comments.Count,
            CreatedAt = post.CreatedAt,
            IsLikedByCurrentUser = currentUserId.HasValue && post.Likes.Any(l => l.UserId == currentUserId)
        };
    }
}
