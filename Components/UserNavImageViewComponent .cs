using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatBox.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeatBox.Areas.Components
{
    public class UserNavImageViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public UserNavImageViewComponent (UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
                return null!;

            return View("Default", user);
        }
    }
}