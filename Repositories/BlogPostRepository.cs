using Bloggi.Data;
using Bloggi.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggi.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggiDbContext bloggiDbContext;
        public BlogPostRepository(BloggiDbContext bloggiDbContext)
        {
            this.bloggiDbContext = bloggiDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
          await bloggiDbContext.AddAsync(blogPost);
            await bloggiDbContext.SaveChangesAsync();
            return blogPost;
        }

        public Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await bloggiDbContext.BlogPosts.Include(X => X.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
          return await bloggiDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
