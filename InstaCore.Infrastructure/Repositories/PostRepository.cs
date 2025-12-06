using InstaCore.Core.Contracts.Repos;
using InstaCore.Core.Models;
using InstaCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace InstaCore.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly InstaCoreDbContext dbContext;

        public PostRepository(InstaCoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task AddAsync(Post post)
        {
            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Post post)
        {
            dbContext.Posts.Remove(post);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Post?> GetByIdAsync(Guid id)
        {
            return await dbContext
                .Posts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Post>> GetByUserAsync(Guid userId, int skip, int take)
        {
            return await dbContext
                .Posts
                .Where(p => p.UserId == userId)
                .Include (p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .Skip(skip)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Post>> GetFeedAsync(IReadOnlyList<Guid> authorIds, int skip, int take)
        {
            if (authorIds.Count == 0)
                return new List<Post>().AsReadOnly();

            return await dbContext
                .Posts
                .Where(p => authorIds.Contains(p.UserId))
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .OrderByDescending (p => p.CreatedAt)
                .Skip(skip)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
