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
            return await dbContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await dbContext.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await 
                dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await 
                dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await 
                dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u =>u.Username.ToLower() == username.ToLower());
        }

        public async Task UpdateAsync(User user)
        {
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }
    }
}
