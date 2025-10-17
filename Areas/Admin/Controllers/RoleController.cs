using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using BeatBox.Areas.Admin.Models;
using BeatBox.Services;
using BeatBox.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeatBox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager,
                                UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToRole(string UserId, string RoleName)
        {
            if (!String.IsNullOrEmpty(UserId) && !String.IsNullOrEmpty(RoleName))
            {
                var user = await _userManager.FindByIdAsync(UserId);

                if (user is not null && await _roleManager.RoleExistsAsync(RoleName))
                {
                    var results = await _userManager.AddToRoleAsync(user, RoleName);

                    if (results.Succeeded)
                    {
                        TempData["message"] = AppMessages.AddToRole;

                        return RedirectToAction("Index", "User");
                    }
                }
            }

            TempData["failedmessage"] = AppMessages.OperationFaild;

            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (roleName is not null)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (result.Succeeded)
                {
                    TempData["message"] = AppMessages.CreateRole;

                    return RedirectToAction("Index", "User");
                }
            }
            TempData["failedmessage"] = AppMessages.OperationFaild;

            return RedirectToAction("Index", "User");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var role = await _roleManager.FindByIdAsync(Id);

                if (role != null)
                {
                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        TempData["message"] = AppMessages.DeleteRole;

                        return RedirectToAction("Index", "User");
                    }
                }
            }

            TempData["failedmessage"] = AppMessages.OperationFaild;

            return RedirectToAction("Index", "User");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdminRole()
        {
            var results = await _roleManager.CreateAsync(new IdentityRole("Admin"));

            if (results.Succeeded)
            {
                TempData["message"] = AppMessages.CreateAdminRole;

                return RedirectToAction("Index", "User");
            }

            TempData["failedmessage"] = AppMessages.OperationFaild;

            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromRole(string UserId, string RoleName)
        {
            if (!String.IsNullOrEmpty(UserId) && !String.IsNullOrEmpty(RoleName))
            {
                var user = await _userManager.FindByIdAsync(UserId);

                var role = await _roleManager.FindByNameAsync(RoleName);

                if (user != null && role != null)
                {
                    var results = await _userManager.RemoveFromRoleAsync(user, RoleName);

                    if (results.Succeeded)
                    {
                        TempData["message"] = AppMessages.RemoveFromRole;

                        return RedirectToAction("Index", "User");
                    }
                }
            }

            TempData["failedmessage"] = AppMessages.OperationFaild;

            return RedirectToAction("Index", "User");
        }
    }
}