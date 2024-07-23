using Bloggi.Models.Domain;
using Bloggi.Models.ViewModels;
using Bloggi.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggi.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository,
            IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository
            )
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandel)
        {
            var blogPost = await blogPostRepository.GetByUrlHandel(urlHandel);
            var blogPostDetails = new BlogPostDetails();
            var liked = false;

            if (blogPost != null)
            {

                var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);
                if (signInManager.IsSignedIn(User))
                {
                    //geting all likes
                    var likesForBlog = await blogPostLikeRepository.GetLikesForBlogPost(blogPost.Id);
                    var userId = userManager.GetUserId(User);
                    if (userId != null)
                    {
                        var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;

                    }

                }
                //get comments for blogpost 
                var blogComments = await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);
                var blogCommentsForView = new List<BlogComment>();
                foreach (var comment in blogComments)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = comment.Description,
                        DateAdded = comment.DateAdded,
                        Username = (await userManager.FindByIdAsync(comment.UserId.ToString())).UserName
                    });
                }

                blogPostDetails = new BlogPostDetails
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishDate = blogPost.PublishDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandel = blogPost.UrlHandel,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = blogCommentsForView

                };
            }
            return View(blogPostDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogPostDetails blogPostDetails)
        {
            if (signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogPostDetails.Id,
                    Description = blogPostDetails.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now

                };
                await blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index" , "Blogs",new {urlHandel = blogPostDetails.UrlHandel });
            }
            return View();

        }

    }
}
