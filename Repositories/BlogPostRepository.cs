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

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
          var existingBlog =  await bloggiDbContext.BlogPosts.FindAsync(id);
            if (existingBlog != null)
            {
                bloggiDbContext.BlogPosts.Remove(existingBlog);
                await bloggiDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await bloggiDbContext.BlogPosts.Include(X => X.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
          return await bloggiDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandel(string urlHandel)
        {
           return await bloggiDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UrlHandel == urlHandel);
           
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await bloggiDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlogPost != null)
            {
                existingBlogPost.Id = blogPost.Id;
                existingBlogPost.Heading = blogPost.Heading;
                existingBlogPost.PageTitle = blogPost.PageTitle;
                existingBlogPost.Content = blogPost.Content;
                existingBlogPost.ShortDescription = blogPost.ShortDescription;
                existingBlogPost.Author = blogPost.Author;
                existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlogPost.UrlHandel = blogPost.UrlHandel;
                existingBlogPost.PublishDate = blogPost.PublishDate;
                existingBlogPost.Visible = blogPost.Visible;
                existingBlogPost.Tags = blogPost.Tags;
                await bloggiDbContext.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;
        }
    }
}
