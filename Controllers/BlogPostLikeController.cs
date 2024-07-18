﻿using Bloggi.Models.Domain;
using Bloggi.Models.ViewModels;
using Bloggi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostLikeRepository blogPostLikeRepository;

        public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
        {
            this.blogPostLikeRepository = blogPostLikeRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
        {
            
                var model = new BlogPostLike
                {
                    BlogPostId = addLikeRequest.BlogPostId,
                    UserId = addLikeRequest.UserId
                };
                await blogPostLikeRepository.AddLikeForBlog(model);
                return Ok();

            
        }
    }
}
