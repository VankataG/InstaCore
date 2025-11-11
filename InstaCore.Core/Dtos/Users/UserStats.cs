using InstaCore.Core.Models;

namespace InstaCore.Core.Dtos.Users
{
    public class UserStats
    {
        public List<User> Followers { get; set; } = new();

        public List<User> Following { get; set; } = new();

        public List<Post> Posts { get; set; } = new();
    }
}
