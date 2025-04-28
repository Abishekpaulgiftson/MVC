using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using mvc_1.Models.Domain;
namespace mvc_1.Data
{
    public class BloggieDbContext:DbContext
    {
        public BloggieDbContext(DbContextOptions<BloggieDbContext> options):base(options)
        {
        }
        public DbSet<Blog_model> models { get; set; }
        public DbSet<tag> tags { get; set; }
        public DbSet<BlogPostLike> BlogPostLike { get; set; }
        public DbSet<BlogPostComment> BlogPostComment { get; set; }
    }
}
