using Microsoft.EntityFrameworkCore;
using mvc_1.Data;
using mvc_1.Models.Domain;

namespace mvc_1.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private BloggieDbContext bloggieDbContext;

        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task<Blog_model> AddAsync(Blog_model blog_Model)
        {
            await bloggieDbContext.AddAsync(blog_Model);
            await bloggieDbContext.SaveChangesAsync();
            return blog_Model;
        }
        public async Task<IEnumerable<Blog_model>> GetAllAsync()
        {
            return await bloggieDbContext.models.Include(x => x.tags).ToListAsync();
        }
        public async Task<Blog_model?> GetAsync(Guid id)
        {
            return await bloggieDbContext.models.Include(x => x.tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Blog_model?> UpdateAsync(Blog_model  blog_Model)
        {
            var existingBlog = await bloggieDbContext.models.Include(x => x.tags).FirstOrDefaultAsync(x => x.Id == blog_Model.Id);
            if (existingBlog != null)
            {
                existingBlog.Id = blog_Model.Id;
                existingBlog.Heading = blog_Model.Heading;
                existingBlog.PageTitle = blog_Model.PageTitle;
                existingBlog.Content = blog_Model.Content;
                existingBlog.ShortDsecription = blog_Model.ShortDsecription;
                existingBlog.FeaturedImgUrl = blog_Model.FeaturedImgUrl;
                existingBlog.UrlHandle = blog_Model.UrlHandle;
                existingBlog.PublishedDate = blog_Model.PublishedDate;
                existingBlog.Author = blog_Model.Author;
                existingBlog.Visible = blog_Model.Visible;
                existingBlog.tags = blog_Model.tags;
            }
            await bloggieDbContext.SaveChangesAsync();
            return existingBlog;
        }
        public async Task<Blog_model?> DeleteAsync(Guid id)
        {
            var existingBlog = await bloggieDbContext.models.FindAsync(id);
            if (existingBlog != null)
            {
                bloggieDbContext.models.Remove(existingBlog);
                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<Blog_model?> GetByUrlHandleAsync(string urlHandle)
        {
            return await bloggieDbContext.models.Include(x => x.tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }
    }
}
