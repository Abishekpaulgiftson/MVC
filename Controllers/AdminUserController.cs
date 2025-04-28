using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mvc_1.Models.View_Models;
using mvc_1.Repositories;
using System.Collections;

namespace mvc_1.Controllers
{
    [Authorize (Roles ="Admin")]
    public class AdminUserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AdminUserController(IUserRepository userRepository,UserManager<IdentityUser>userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task <IActionResult> List()
        {
            var users = await userRepository.GetAll();
            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();
            foreach (var user in users)
            {
                usersViewModel.Users.Add(new Models.View_Models.User
                {
                    Id = Guid.Parse(user.Id),
                    Username = user.UserName,
                    EmailAddress = user.Email
                });
            }
                return View(usersViewModel);       
        }
        [HttpPost]
        public async Task<IActionResult>List(UserViewModel request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email
            };
            var identityResult = await userManager.CreateAsync(identityUser, request.Password);
            if(identityResult is not null)
            {
                if(identityResult.Succeeded)
                {
                    var roles = new List<string> { "User" };
                    if(request.AdminRoleCheckbox)
                    {
                        roles.Add("Admin");
                    }
                    identityResult = await userManager.AddToRolesAsync(identityUser, roles);
                    if(identityResult is not null && identityResult.Succeeded)
                    {
                        return RedirectToAction("List", "AdminUser");
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Delete(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if(user!=null)
            {
                var identityResult = await userManager.DeleteAsync(user);
                if(identityResult is not null && identityResult.Succeeded)
                {
                    return RedirectToAction("List", "AdminUser");
                }
            }
            return View();
        }

    }
}
