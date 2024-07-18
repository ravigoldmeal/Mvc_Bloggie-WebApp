using Bloggi.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggi.Data
{
    public class BloggiDbContext : DbContext
    {
        public BloggiDbContext(DbContextOptions<BloggiDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostLike> BlogPostLike { get; set; }
    }
}
