namespace InstaCore.Infrastructure.Models
{
    public class Follow
    {
        public Guid FollowerId { get; set; }

        public required User Follower { get; set; }

        public Guid FolloweeId { get; set; }

        public required User Followee { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
