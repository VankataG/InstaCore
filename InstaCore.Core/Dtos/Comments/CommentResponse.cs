namespace InstaCore.Core.Dtos.Comments
{
    public class CommentResponse
    {
        public Guid CommentId { get; set; }

        public Guid PostId { get; set; }

        public string Username { get; set; } = null!;

        public string Text { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
