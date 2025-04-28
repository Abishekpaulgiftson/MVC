using Microsoft.AspNetCore.Mvc;
using mvc_1.Models;
using mvc_1.Models.Domain;
using mvc_1.Models.View_Models;
using mvc_1.Repositories;
using System.Diagnostics;

namespace mvc_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;

        public HomeController(ILogger<HomeController> logger,IBlogPostRepository blogPostRepository,ITagRepository tagRepository)
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
        }

        public async Task <IActionResult> Index()
        {
            var blog_Model=await blogPostRepository.GetAllAsync();
            var tags=await tagRepository.GetAllAsync();
            var model = new HomeViewModel
            {
                Blog_model = blog_Model,
                tag = tags
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
