namespace InstaCore.Infrastructure.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }

        public required User User { get; set; }

        public required string Caption { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<Like> Likes { get; set; } = new HashSet<Like>();
    }
}
