namespace InstaCore.Core.Dtos.Likes
{
    public class LikeResponse
    {
        public Guid? LikeId { get; set; }

        public Guid PostId { get; set; }

        public string Username { get; set; } = null!;

        public bool Liked { get; set; }

        public int TotalLikes { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
