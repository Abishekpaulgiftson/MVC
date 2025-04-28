using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mvc_1.Models.Domain;
using mvc_1.Models.View_Models;
using mvc_1.Repositories;

namespace mvc_1.Controllers
{
    public class BlogsController : Controller
    { 
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public BlogsController(IBlogPostRepository blogPostRepository,SignInManager<IdentityUser>signInManager,UserManager<IdentityUser>userManager,IBlogPostLikeRepository blogPostLikeRepository,IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.blogPostCommentRepository = blogPostCommentRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task <IActionResult> Index( string urlHandle)
        {
            var blog_Model = await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            var blogDetailsViewModel = new BlogDetailsViewModels();
            var liked = false;
            if (blog_Model != null)
            {
                var totalLikes = await blogPostLikeRepository.GetTotalLikes(blog_Model.Id);
                if (signInManager.IsSignedIn(User))
                {
                    //Get like for this blog for this user
                    var likesForBlog = await blogPostLikeRepository.GetLikesForBlog(blog_Model.Id);
                    var userId = userManager.GetUserId(User);
                    if (userId != null)
                    {
                        var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }
                }
                //Get comments for blog post
                var blogCommentsDomainModel = await blogPostCommentRepository.GetCommentsByBlogIdAsync(blog_Model.Id);
                var blogCommentsForView = new List<BlogComment>();
                foreach (var blogComment in blogCommentsDomainModel)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        Username = (await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                    });
                }

                blogDetailsViewModel = new BlogDetailsViewModels
                {
                    Id = blog_Model.Id,
                    Heading = blog_Model.Heading,
                    PageTitle = blog_Model.PageTitle,
                    Content = blog_Model.Content,
                    ShortDescription = blog_Model.ShortDsecription,
                    FeaturedImageUrl = blog_Model.FeaturedImgUrl,
                    UrlHandle = blog_Model.UrlHandle,
                    Author = blog_Model.Author,
                    PublishedDate = blog_Model.PublishedDate,
                    Visible = blog_Model.Visible,
                    Tags = blog_Model.tags,
                    Liked = liked
                };
            }
            return View(blogDetailsViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModels blogDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };
                await blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandle });
            }
            return View();
        }
    }
}
