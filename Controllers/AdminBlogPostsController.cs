using Bloggi.Models.Domain;
using Bloggi.Models.ViewModels;
using Bloggi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggi.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;
        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        public ITagRepository TagRepository { get; }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //get tags from repository
            var tags = await tagRepository.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString() })
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            //map view model to domain model
            var blogpost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandel = addBlogPostRequest.UrlHandel,
                PublishDate = addBlogPostRequest.PublishDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,

            };
            //map tags from selected tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
                if (existingTag != null)
                {
                    {
                        selectedTags.Add(existingTag);
                    }
                }
            }
            blogpost.Tags = selectedTags;
            await blogPostRepository.AddAsync(blogpost); 
            return RedirectToAction("Add");
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {  
            var blogposts = await blogPostRepository.GetAllAsync();

            return View(blogposts);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // Retrieve result from repository
            var blogPost = await blogPostRepository.GetAsync(id);
            var tagDomainModel = await tagRepository.GetAllAsync();

            if (blogPost == null)
            {
                // Handle the case where the blog post is not found
                return View(null);
            }

            // Map domain model to view model
            var model = new EditBlogPostRequest
            {
                Id = blogPost.Id,
                Heading = blogPost.Heading,
                PageTitle = blogPost.PageTitle,
                Content = blogPost.Content,
                Author = blogPost.Author,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandel = blogPost.UrlHandel,
                ShortDescription = blogPost.ShortDescription,
                PublishDate = blogPost.PublishDate,
                Visible = blogPost.Visible,
                Tags = tagDomainModel.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
            };

            // Pass data to view 
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //map view model to domain model 
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                PublishDate = editBlogPostRequest.PublishDate,
                UrlHandel = editBlogPostRequest.UrlHandel,
                Visible = editBlogPostRequest.Visible
            };

            var selectedTags = new List<Tag>();
            foreach(var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }
            blogPostDomainModel.Tags = selectedTags;


            //submit info to repository 
            var Updatedblog = await blogPostRepository.UpdateAsync(blogPostDomainModel);
            if(Updatedblog != null)
            {
                return RedirectToAction("List");
            }
            //redirect to get
            return RedirectToAction("Edit");

        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
           var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
            if(deletedBlogPost != null)
            {

            return RedirectToAction("List"); 
            }
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
             
        }

    }



}
