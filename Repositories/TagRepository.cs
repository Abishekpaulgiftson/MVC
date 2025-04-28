using Microsoft.EntityFrameworkCore;
using mvc_1.Data;
using mvc_1.Models.Domain;
using System.Threading.Tasks;

namespace mvc_1.Repositories
{
    public class TagRepository :ITagRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public TagRepository(BloggieDbContext bloggieDbContext1)
        {
            this.bloggieDbContext = bloggieDbContext1;
        }
        public async Task<IEnumerable<tag>> GetAllAsync()
        {
            return await bloggieDbContext.tags.ToListAsync();
        }
        public async Task<tag> GetAsync(Guid id)
        {
            return bloggieDbContext.tags.FirstOrDefault(x => x.id == id);
        }

        public async Task<tag>AddAsync(tag tag)
        {
            await bloggieDbContext.tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();
            return tag;
        }
        public async Task<tag> UpdateAsync(tag tag)
        {
            var existingTag=await bloggieDbContext.tags.FindAsync(tag.id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                await bloggieDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }
        public async Task<tag?> DeleteAsync(Guid id)
        {
            var existingTag = await bloggieDbContext.tags.FindAsync(id);
            if (existingTag != null)
            {
                bloggieDbContext.tags.Remove(existingTag);
                await bloggieDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }
    }
}
