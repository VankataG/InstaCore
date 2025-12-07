using InstaCore.Core.Dtos.Comments;
using InstaCore.Core.Models;

namespace InstaCore.Core.Mapping
{
    public static class CommentMapping
    {
        public static CommentResponse ToResponse(Comment comment) => new()
        {
            CommentId = comment.Id,
            PostId = comment.PostId,
            Username = comment.User.Username,
            Text = comment.Text,
            CreatedAt = comment.CreatedAt
        };

        public static CommentResponse ToResponse(Comment comment, User user) => new()
        {
            CommentId = comment.Id,
            PostId = comment.PostId,
            Username = user.Username,
            Text = comment.Text,
            CreatedAt = comment.CreatedAt
        };
    }
}
