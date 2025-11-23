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
            Caption = post.Caption,
            ImageUrl = post.ImageUrl,
            Likes = post.Likes.Count,
            Comments = post.Comments.Count,
            CreatedAt = post.CreatedAt
        };
    }
}
