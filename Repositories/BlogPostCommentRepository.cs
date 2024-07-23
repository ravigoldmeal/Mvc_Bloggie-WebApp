using Bloggi.Data;
using Bloggi.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggi.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BloggiDbContext bloggiDbContext;

        public BlogPostCommentRepository(BloggiDbContext bloggiDbContext)
        {
            this.bloggiDbContext = bloggiDbContext;
        }
        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await bloggiDbContext.BlogPostComment.AddAsync(blogPostComment);
            await bloggiDbContext.SaveChangesAsync();
            return blogPostComment;
        }
    

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
            return await bloggiDbContext.BlogPostComment.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
