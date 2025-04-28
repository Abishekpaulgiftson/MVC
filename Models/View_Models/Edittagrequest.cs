using mvc_1.Models.Domain;

namespace mvc_1.Models.View_Models
{
    public class Edittagrequest
    {
        public Guid id { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public ICollection<Blog_model> blog_Models { get; set; }

    }
}
