namespace InstaCore.Core.Dtos.Posts
{
    public class CreatePostRequest
    {
        public required string Caption { get; set; }

        public string? ImageUrl { get; set; }
    }
}
