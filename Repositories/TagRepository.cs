using Bloggi.Data;
using Bloggi.Models.Domain;
using Bloggi.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bloggi.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BloggiDbContext _bloggiDbContext;
        public TagRepository(BloggiDbContext bloggiDbContext)
        {
           _bloggiDbContext = bloggiDbContext;
       
        }

       

       async Task<Tag> ITagRepository.AddAsync(Tag tag)
        {
            await _bloggiDbContext.Tags.AddAsync(tag);
            await _bloggiDbContext.SaveChangesAsync();
            return tag;
        }

         async Task<Tag?> ITagRepository.DeleteAsync(Guid id)
        {
          var existingTag =   await _bloggiDbContext.Tags.FindAsync(id);
            if(existingTag != null)
            {
                _bloggiDbContext.Tags.Remove(existingTag);
                await _bloggiDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }

         async Task<IEnumerable<Tag>> ITagRepository.GetAllAsync()
        {
            return await _bloggiDbContext.Tags.ToListAsync();
        }

         async Task<Tag?> ITagRepository.GetAsync(Guid id)
        {
          return await _bloggiDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
        }

        async Task<Tag?> ITagRepository.UpdateAsync(Tag tag)
        {
            var existingTag = await _bloggiDbContext.Tags.FindAsync(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                await _bloggiDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
           
        }
    }
}
