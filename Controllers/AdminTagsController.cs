using Bloggi.Data;
using Bloggi.Models.Domain;
using Bloggi.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggi.Controllers
{   
    public class AdminTagsController : Controller
    {
        private BloggiDbContext _bloggiDbContext;
        public AdminTagsController(BloggiDbContext bloggiDbContext)
        {
            _bloggiDbContext = bloggiDbContext;


        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        //[ActionName("Add")]
        public IActionResult /*SubmitTag*/ Add(AddTagRequest addTagRequest)
        {
            // mapping add tag to tag domain model
            var tag =new Tag
            { 
                Name = addTagRequest.Name , 
                DisplayName = addTagRequest.DisplayName ,
            };
            _bloggiDbContext.Tags.Add(tag);
            _bloggiDbContext.SaveChanges();

            return RedirectToAction("List");
        }
        [HttpGet]
        [ActionName("List")]
        public IActionResult List()
        {
            var tags = _bloggiDbContext.Tags.ToList();
            return View(tags);
        }
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
           //var tag = _bloggiDbContext.Tags.Find(id);
          var tag =  _bloggiDbContext.Tags.FirstOrDefault(t => t.Id == id);
            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }
            return View(null);
        }
        [HttpPost]
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            var existingTag = _bloggiDbContext.Tags.Find(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                _bloggiDbContext.SaveChanges();
                return RedirectToAction("List", new { id = editTagRequest.Id });
            }
            return RedirectToAction("Edit" , new { id = editTagRequest.Id });
        }
        [HttpPost]
        public IActionResult Delete(EditTagRequest editTagRequest)
        {
         var tag = _bloggiDbContext.Tags.Find(editTagRequest.Id);
            if(tag != null)
            {
                _bloggiDbContext.Tags.Remove(tag);
                _bloggiDbContext.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit",new { id = editTagRequest.Id });
        }


            

    }

           
    
}
