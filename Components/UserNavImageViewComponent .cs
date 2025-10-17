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
            {
                var defaultUser = new AppUser
                {
                    UserName = "Guest",
                    Id = "0",
                    PictureUrl = "/images/default_image.png",
                    NavBarPicture = "/images/Navbar_images/default_nav_image.png",
                    ProfilePicture = "/images/profile_images/default_profile_image.png"
                };
                return View("Default", defaultUser);
            }

            return View("Default", user);
        }
    }
}