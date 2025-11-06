namespace InstaCore.Infrastructure.Models
{
    public class Post
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public required string Caption { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
