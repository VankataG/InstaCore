namespace InstaCore.Core.Dtos.Posts
{
    public class PostResponse
    {
        public Guid Id { get; set; }

        public required string Username { get; set; }

        public string? UserAvatarUrl { get; set; }

        public string? Caption { get; set; }

        public string? ImageUrl { get; set; }

        public int Likes { get; set; }

        public int Comments { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsLikedByCurrentUser { get; set; }

    }
}
