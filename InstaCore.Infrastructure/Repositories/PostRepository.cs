using InstaCore.Core.Contracts.Repos;
using InstaCore.Core.Models;
using InstaCore.Data;
using Microsoft.EntityFrameworkCore;

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

        public Task<IReadOnlyList<Post>> GetByUserAsync(Guid userId, int skip, int take)
        {
            throw new NotImplementedException();
        }
    }
}
