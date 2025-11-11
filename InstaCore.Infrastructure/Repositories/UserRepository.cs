using InstaCore.Core.Contracts;
using InstaCore.Core.Models;
using InstaCore.Data;
using Microsoft.EntityFrameworkCore;

namespace InstaCore.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly InstaCoreDbContext dbContext;

        public UserRepository(InstaCoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task AddAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await dbContext.Users.AnyAsync(u => u.Email.ToLowerInvariant() == email.ToLowerInvariant());
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await dbContext.Users.AnyAsync(u => u.Username.ToLowerInvariant() == username.ToLowerInvariant());
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await 
                dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.ToLowerInvariant() == email.ToLowerInvariant());
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await 
                dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u =>u.Username.ToLowerInvariant() == username.ToLowerInvariant());
        }
    }
}
