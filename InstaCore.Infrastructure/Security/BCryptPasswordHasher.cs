using InstaCore.Core.Contracts;

namespace InstaCore.Infrastructure.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            throw new NotImplementedException();
        }

        public bool Verify(string password, string passwordHash)
        {
            throw new NotImplementedException();
        }
    }
}
