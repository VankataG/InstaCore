namespace InstaCore.Core.Dtos.Users
{
    public class UserResponse
    {
        public Guid Id { get; set; }

        public required string Username { get; set; }

        public string? Bio { get; set; }

        public string? AvatarUrl { get; set; }
    }
}
