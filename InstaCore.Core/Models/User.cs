namespace InstaCore.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public required string Username { get; set; }

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public string? Bio { get; set; }
        
        public string? AvatarUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<Like> Likes { get; set; } = new HashSet<Like>();

        public virtual ICollection<Follow> Followers { get; set; } = new HashSet<Follow>();

        public virtual ICollection<Follow> Following { get; set; } = new HashSet<Follow>();

    }
}
