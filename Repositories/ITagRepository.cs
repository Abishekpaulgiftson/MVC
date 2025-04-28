using mvc_1.Models.Domain;

namespace mvc_1.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<tag>> GetAllAsync();
        Task<tag?>GetAsync(Guid id);
        Task<tag>AddAsync(tag tag);
        Task<tag?>UpdateAsync(tag tag);
        Task<tag?>DeleteAsync(Guid id);
    }
}
