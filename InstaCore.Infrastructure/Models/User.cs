namespace InstaCore.Infrastructure.Models
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

    }
}
