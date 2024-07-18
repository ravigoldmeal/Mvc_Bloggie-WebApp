﻿using Bloggi.Models.ViewModels;
using Bloggi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bloggi.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;

        public BlogsController(IBlogPostRepository blogPostRepository,IBlogPostLikeRepository blogPostLikeRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
        }
        [HttpGet]
        public async Task< IActionResult >Index(string urlHandel)
        {
            var blogPost = await blogPostRepository.GetByUrlHandel(urlHandel);
            var blogPostLikeDetails = new BlogPostDetails();

            if (blogPost != null)
            {

               var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);
                blogPostLikeDetails = new BlogPostDetails
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
                    TotalLikes = totalLikes

                };
            }
            return View(blogPostLikeDetails);
        }

    }
}
