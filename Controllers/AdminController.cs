using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_1.Data;
using mvc_1.Models.Domain;
using mvc_1.Models.View_Models;
using mvc_1.Repositories;

namespace mvc_1.Controllers
{
    [Authorize (Roles ="Admin")]
    public class AdminController : Controller

    {
        private int id;

        public ITagRepository TagRepository { get; }

        public AdminController(ITagRepository TagRepository)
        {
            this.TagRepository = TagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        // [ActionName("Add")]
        public async Task<IActionResult> Add(Addtagrequest addtagrequest)
        {
            ValidateAddTagRequest(addtagrequest);
            if (ModelState.IsValid==false)
            {
                return View();
            }
            var tag = new tag
            {
                Name = addtagrequest.Name,
                DisplayName = addtagrequest.DisplayName
            };

            await TagRepository.AddAsync(tag);
            return RedirectToAction("List");
        }
        [HttpGet]
        //[ActionName("List")]
        public async Task<IActionResult> List()
        {
            var tags = await TagRepository.GetAllAsync();
            return View(tags);
        }
        [HttpGet]
        // [ActionName("displaying details for edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await TagRepository.GetAsync(id);
            if (tag != null)
            {
                var edittagrequest = new Edittagrequest
                {
                    id = tag.id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(edittagrequest);
            }
            return View(null);
        }
        [HttpPost]
        //  [ActionName("for submitting the edited details")]
        public async Task<IActionResult> Edit(Edittagrequest edittagrequest)
        {
            var tag = new tag
            {
                id = edittagrequest.id,
                Name = edittagrequest.Name,
                DisplayName = edittagrequest.DisplayName
            };
            var updatedTag = await TagRepository.UpdateAsync(tag);
            if (updatedTag != null)
            {
                //show success notification
            }
            else
            {
                //show error notification
            }

            return RedirectToAction("Edit", new { id = edittagrequest.id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Edittagrequest edittagrequest)
        {
            var tag = await TagRepository.DeleteAsync(edittagrequest.id);
            if (tag != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = edittagrequest.id });
        }
        private void ValidateAddTagRequest(Addtagrequest request)
        {
            if (request.Name is not null && request.DisplayName is not null)
            {
                if (request.Name == request.DisplayName)
                {
                    ModelState.AddModelError("DisplayName", "Name cannot be same as display name");
                }
            }
        }
    }
}
    

        

    