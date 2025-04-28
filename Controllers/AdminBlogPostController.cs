using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using mvc_1.Models.Domain;
using mvc_1.Models.View_Models;
using mvc_1.Repositories;

namespace mvc_1.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminBlogPostController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;

        public AdminBlogPostController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }
        [HttpGet]
        public async Task <IActionResult> Add()
        {
            var tags=await tagRepository.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.id.ToString() })
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult>Add(AddBlogPostRequest addBlogPostRequest)
        {
            var blog_Model = new Blog_model
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDsecription = addBlogPostRequest.ShortDescription,
                FeaturedImgUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,
            };
            var selectedTags = new List<tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }
            blog_Model.tags = selectedTags;
            await blogPostRepository.AddAsync(blog_Model);
                return RedirectToAction("Add");
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            //call the repository
            var blog_Model = await blogPostRepository.GetAllAsync();
            return View(blog_Model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //retrieve the result from the repository
            var blog_Model = await blogPostRepository.GetAsync(id);
            var tagsDomainModel = await tagRepository.GetAllAsync();
            if (blog_Model!=null)
            {
                //map the domain model into the view model
                var model = new EditBlogPostRequest
                {
                    Id = blog_Model.Id,
                    Heading = blog_Model.Heading,
                    PageTitle = blog_Model.PageTitle,
                    Content =blog_Model.Content,
                    ShortDescription = blog_Model.ShortDsecription,
                    FeaturedImageUrl = blog_Model.FeaturedImgUrl,
                    UrlHandle = blog_Model.UrlHandle,
                    PublishedDate = blog_Model.PublishedDate,
                    Author = blog_Model.Author,
                    Visible = blog_Model.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.id.ToString()
                    }),
                    SelectedTags = blog_Model.tags.Select(x => x.id.ToString()).ToArray()
                };

                //pass data to view
                return View(model);
            }

            return View(null);

        }
         [HttpPost]
        public async Task<IActionResult>Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //map view model back to domain model
            var blog_modelDomainModel = new Blog_model
            {
                Id = editBlogPostRequest.Id,    
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                ShortDsecription = editBlogPostRequest.ShortDescription,
                FeaturedImgUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Author = editBlogPostRequest.Author,
                Visible = editBlogPostRequest.Visible
            };
            // map tags to domain model
            var selectedTags=new List<tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }
            blog_modelDomainModel.tags = selectedTags;
            //submit information to repository to update
            var updatedBlog=await blogPostRepository.UpdateAsync(blog_modelDomainModel);
            if (updatedBlog != null)
            {
                //show success notification
                return RedirectToAction("Edit");
            }
            //show error notification
            return RedirectToAction("Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            //talk to repository to delete the blog posts and tags
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
            if (deletedBlogPost != null)
            {
                //show success notification
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
        }

    }
}
