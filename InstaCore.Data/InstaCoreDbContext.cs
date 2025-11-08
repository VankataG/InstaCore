using System.Reflection;
using InstaCore.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InstaCore.Data
{
    public class InstaCoreDbContext : DbContext
    {
        public InstaCoreDbContext(DbContextOptions<InstaCoreDbContext> options) : base(options) { }
        
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Like> Likes { get; set; }

        public virtual DbSet<Post> Posts { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<Follow> Follows { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
