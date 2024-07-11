using Bloggi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bloggi.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }
        [HttpGet]
        public async Task< IActionResult >Index(string urlHandel)
        {
            var blogPost = await blogPostRepository.GetByUrlHandel(urlHandel);
            return View(blogPost);
        }
    }
}
