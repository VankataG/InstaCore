namespace InstaCore.Core.Dtos.Posts
{
    public class PostResponse
    {
        public Guid Id { get; set; }

        public required string Username { get; set; }

        public required string Caption { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}
