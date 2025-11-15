using InstaCore.Core.Models;
using InstaCore.Data;
using Microsoft.EntityFrameworkCore;

namespace InstaCore.Core.Contracts
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
            return await dbContext.Users.AnyAsync(u => u.Email == email.ToLower());
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await dbContext.Users.AnyAsync(u => u.Username == username.Trim());
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await 
                dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email.ToLower());
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await 
                dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByIdWithFollowsAsync(Guid id)
        {
            return await
                dbContext
                .Users
                .Include(u => u.Followers)
                .Include(u => u.Following)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await 
                dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u =>u.Username == username.Trim());
        }

        public async Task<User?> GetByUsernameWithFollowsAsync(string username)
        {
            return await
                dbContext
                .Users
                .Include(u => u.Followers)
                .Include(u => u.Following)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username.Trim());
        }

        public async Task UpdateAsync(User user)
        {
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }
    }
}
