using InstaCore.Core.Contracts.Repos;
using InstaCore.Core.Models;
using InstaCore.Data;
using Microsoft.EntityFrameworkCore;

namespace InstaCore.Infrastructure.Repositories
{
    public class FollowRepository : IFollowRepository
    {

        private readonly InstaCoreDbContext dbContext;

        public FollowRepository(InstaCoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task AddAsync(Follow follow)
        {
            await dbContext.Follows.AddAsync(follow);
            await dbContext.SaveChangesAsync();
        }

        public async Task<int> CountFollowerAsync(Guid userId)
        {
            return await dbContext
                .Follows
                .AsNoTracking()
                .Where(f => f.FolloweeId == userId)
                .CountAsync();
        }

        public async Task<int> CountFollowingAsync(Guid userId)
        {
            return await dbContext
                .Follows
                .AsNoTracking()
                .Where(f => f.FollowerId == userId)
                .CountAsync();
        }

        public async Task<bool> ExistsAsync(Guid followerId, Guid followeeId)
        {
            return await dbContext
                        .Follows
                        .AsNoTracking()
                        .AnyAsync(f => f.FollowerId == followerId &&
                                       f.FolloweeId == followeeId);
        }

        public async Task RemoveAsync(Guid followerId, Guid followeeId)
        {
            Follow? follow = await dbContext
                                  .Follows
                                  .FirstOrDefaultAsync(f => f.FollowerId == followerId &&
                                                       f.FolloweeId == followeeId);

            if (follow != null)
            {
                dbContext.Follows.Remove(follow);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
