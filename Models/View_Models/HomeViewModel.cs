using mvc_1.Models.Domain;

namespace mvc_1.Models.View_Models
{
    public class HomeViewModel
    {
        public IEnumerable<Blog_model> Blog_model{ get; set; }
        public IEnumerable<tag> tag { get; set; }
    }
}
