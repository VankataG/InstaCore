namespace InstaCore.Core.Models
{
    public class Like
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }

        public Post Post { get; set; } = null!;

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
