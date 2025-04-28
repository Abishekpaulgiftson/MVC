using mvc_1.Models.Domain;

namespace mvc_1.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<Blog_model>> GetAllAsync();
        Task<Blog_model?>GetAsync(Guid id);
        Task<Blog_model?> GetByUrlHandleAsync(string urlHandle);
        Task<Blog_model> AddAsync(Blog_model blog_Model);
        Task<Blog_model?> UpdateAsync(Blog_model blog_Model);
        Task<Blog_model?> DeleteAsync(Guid id);
    }
}
