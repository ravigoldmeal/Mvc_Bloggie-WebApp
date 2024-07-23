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

        public async Task<int> CountAsync()
        {
            return await _bloggiDbContext.Tags.CountAsync();
            
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

         async Task<IEnumerable<Tag>> ITagRepository.GetAllAsync(
             string? searchQuery,
             string? sortBy,
             string? sortDirection,
             int pageNumber = 1,
             int pageSize = 100
             )
        {
            var query = _bloggiDbContext.Tags.AsQueryable();
            //filtering
            if(string.IsNullOrWhiteSpace(searchQuery)== false)
            {
                query = query.Where(x => x.Name.Contains(searchQuery) ||
                                           x.DisplayName.Contains(searchQuery));
                
            }

            //sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection,"Desc" , StringComparison.OrdinalIgnoreCase);
                if(string.Equals(sortBy , "Name", StringComparison.OrdinalIgnoreCase))
                {
                query = isDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);

                }      
                if(string.Equals(sortBy , "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                query = isDesc ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName);

                }
            }


            //Pagination
            //
            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);


            return await query.ToListAsync();
          //  return await _bloggiDbContext.Tags.ToListAsync();
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
