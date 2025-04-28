using Microsoft.EntityFrameworkCore;

namespace mvc_1.Models.Domain
{
    public class tag
    {
        public Guid id { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public ICollection<Blog_model> blog_Models { get; set; }

    }
}
