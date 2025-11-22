using InstaCore.Core.Contracts.Repos;
using InstaCore.Core.Models;
using InstaCore.Data;
using Microsoft.EntityFrameworkCore;

namespace InstaCore.Infrastructure.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly InstaCoreDbContext dbContext;

        public LikeRepository(InstaCoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task AddAsync(Like like)
        {
            await dbContext.Likes.AddAsync(like);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Like like)
        {
            dbContext.Likes.Remove(like);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Like?> GetByUserAndPostAsync(Guid userId, Guid postId)
        {
            return await dbContext
                .Likes
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);
        }
    }
}
