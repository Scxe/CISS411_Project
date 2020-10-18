using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CISS411_Project.Models;
using CISS411_Project.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CISS411_Project.Controllers
{
    public class AccountController : Controller
    {
        private SwimDbContext db;
        private UserManager<RegisteredUser> userManager;
        private SignInManager<RegisteredUser> signInManager;
        private RoleManager<IdentityRole> roleManager;
        public AccountController(UserManager<RegisteredUser> userManager,
            SignInManager<RegisteredUser> signInManager,
            RoleManager<IdentityRole> roleManager, SwimDbContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.db = db;
        }
        // Registration
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new RegisteredUser { UserName = vm.Email, Email = vm.Email };
                var result = await userManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(vm);
        }
        // Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(vm.Email, vm.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(vm.Email);
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (roles.Contains("Coach"))
                    {
                        return RedirectToAction("Index", "Coach");
                    }
                    else if (roles.Contains("Swimmer"))
                    {
                        return RedirectToAction("Index", "Swimmer");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Login Failure.");
            }
            return View(vm);
        }
        // Display all users
        [Authorize(Roles="Admin")] // Admin-only area to simplify design. AT 10-15-20
        public IActionResult AllUser()
        {
            var users = db.Users.ToList();
            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}