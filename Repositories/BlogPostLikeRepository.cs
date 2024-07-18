
using Bloggi.Data;
using Bloggi.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggi.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BloggiDbContext bloggiDbContext;

        public BlogPostLikeRepository(BloggiDbContext bloggiDbContext)
        {
            this.bloggiDbContext = bloggiDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await bloggiDbContext.BlogPostLike.AddAsync(blogPostLike);
            await bloggiDbContext.SaveChangesAsync();  
            return blogPostLike;
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
           return await bloggiDbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);
        }
    }
}
