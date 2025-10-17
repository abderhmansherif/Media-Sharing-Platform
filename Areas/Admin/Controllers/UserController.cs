using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatBox.Models;
using Microsoft.AspNetCore.Identity;
using BeatBox.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using BeatBox.Areas.Admin.Models;
using BeatBox.Services;

namespace BeatBox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public UserController(UserManager<AppUser> userManger,
                             RoleManager<IdentityRole> roleManager,
                            SignInManager<AppUser> signInManager)
        {
            _userManager = userManger;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            List<AppUser> users = new List<AppUser>();
            List<AppUser> UsersFromDb = _userManager.Users.ToList();

            foreach (var user in UsersFromDb)
            {
                user.RoleNames = await _userManager.GetRolesAsync(user);
                users.Add(user);
            }

            List<IdentityRole> roles = _roleManager.Roles.ToList();

            UserViewModel userViewModels = new UserViewModel
            {
                Users = users,
                Roles = roles
            };
           
            return View(userViewModels);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string Id)
        {
            if (Id is not null)
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user is not null)
                {
                    if (User.Identity.IsAuthenticated && User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value == user.Id)
                    {
                        return RedirectToAction("index", "User");
                    }

                    var results = await _userManager.DeleteAsync(user);

                    if (results.Succeeded)
                    {
                        TempData["message"] = AppMessages.DeleteUser;

                        return RedirectToAction("index", "User");
                    }
                }
            }

            TempData["failedmessage"] = AppMessages.OperationFaild;
            
            return RedirectToAction("index", "User");
        }
       
    }
}