﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mvc_1.Models.View_Models;

namespace mvc_1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser>signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                var IdentityUser = new IdentityUser
                {
                    UserName = registerViewModel.Username,
                    Email=registerViewModel.Email,
                    PasswordHash=registerViewModel.Password
                };
                var identityResult = await userManager.CreateAsync(IdentityUser, registerViewModel.Password);
                if(identityResult.Succeeded)
                {
                    var roleIdentityResult = await userManager.AddToRoleAsync(IdentityUser, "User");
                    if(roleIdentityResult.Succeeded)
                    {
                        return RedirectToAction("Register");
                    }
                }
            }
            return View("Register");
        }
        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = ReturnUrl
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult>Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password,false,false);
            if(signInResult !=null && signInResult.Succeeded)
            {
                if(!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult>Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
