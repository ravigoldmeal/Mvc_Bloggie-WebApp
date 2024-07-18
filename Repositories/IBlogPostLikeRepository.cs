using Bloggi.Models.Domain;

namespace Bloggi.Repositories
{
    public interface IBlogPostLikeRepository
    {
      Task<int>  GetTotalLikes(Guid blogPostId);

        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike );
    }
}
