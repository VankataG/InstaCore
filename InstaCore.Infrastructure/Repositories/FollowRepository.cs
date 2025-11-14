using InstaCore.Core.Contracts;
using InstaCore.Core.Models;
using InstaCore.Data;
using Microsoft.EntityFrameworkCore;

namespace InstaCore.Infrastructure.Repositories
{
    public class FollowRepository : IFollowRepository
    {

        private readonly InstaCoreDbContext dbContext;

        private readonly IUserRepository userRepository;

        public FollowRepository(InstaCoreDbContext dbContext, IUserRepository userRepository)
        {
            this.dbContext = dbContext;
            this.userRepository = userRepository;
        }


        public async Task AddAsync(Follow follow)
        {
            dbContext.Follows.Add(follow);
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
            if (await this.ExistsAsync(followerId, followeeId)){
                Follow follow = await dbContext
                                      .Follows
                                      .AsNoTracking()
                                      .FirstAsync(f => f.FollowerId == followerId &&
                                                       f.FolloweeId == followeeId);

                dbContext.Follows.Remove(follow);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
