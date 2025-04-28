using Microsoft.EntityFrameworkCore;

namespace mvc_1.Models.Domain
{
    public class Blog_model
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }

        public string ShortDsecription { get; set; }
        public string FeaturedImgUrl { get; set; }
        public string UrlHandle { get; set; }

        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }
        public ICollection<tag> tags{ get; set; }
        public ICollection<BlogPostLike> blogPostLikes { get; set; }
        public ICollection<BlogPostComment> blogPostComments { get; set; }
    }
}
