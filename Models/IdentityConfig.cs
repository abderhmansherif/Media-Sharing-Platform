using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using BeatBox.Areas.Admin.Models;
using BeatBox.Areas.Media.Models;
using Microsoft.EntityFrameworkCore;


namespace BeatBox.Models
{
    public static class IdentityConfig
    {
        public static async Task CreatAdminUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var username = "Abderhman202";
            var Email = "bebosherif202@gmail.com";
            var password = "Abderhman2026";
            var RoleName = "Admin";

            using (var context = serviceProvider.GetRequiredService<AppDbContext>())
            {
                await context.Database.EnsureDeletedAsync();

                await context.Database.EnsureCreatedAsync();

                if (await RoleManager.FindByNameAsync(RoleName) is null)
                {
                    await RoleManager.CreateAsync(new IdentityRole { Name = RoleName });
                }

                if (await userManager.FindByNameAsync(username) is null)
                {
                    AppUser user = new AppUser
                    {
                        UserName = username,
                        Email = Email,
                        PictureUrl = "/Uploads/Images/default_image.png",
                        NavBarPicture = "/Uploads/Images/navs/default_nav_image.png",
                        ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png",
                    };

                    var results = await userManager.CreateAsync(user, password);

                    if (results.Succeeded)
                        await userManager.AddToRoleAsync(user, RoleName);
                }


                if (context.Users.Count() == 1 )
                {
                    var users = new List<AppUser>
                    {
                        new AppUser
                        {
                            UserName = "john_doe",
                            Email = "john.doe@example.com",
                            PictureUrl = "/Uploads/Images/default_image.png",
                            NavBarPicture = "/Uploads/Images/navs/default_nav_image.png",
                            ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png"
                        },
                        new AppUser
                        {
                            UserName = "jane_smith",
                            Email = "jane.smith@example.com",
                            PictureUrl = "/Uploads/Images/default_image.png",
                            NavBarPicture = "/Uploads/Images/navs/default_nav_image.png",
                            ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png"
                        },
                        new AppUser
                        {
                            UserName = "michael_brown",
                            Email = "michael.brown@example.com",
                            PictureUrl = "/Uploads/Images/default_image.png",
                            NavBarPicture = "/Uploads/Images/navs/default_nav_image.png",
                            ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png"
                        },
                        new AppUser
                        {
                            UserName = "emily_jones",
                            Email = "emily.jones@example.com",
                            PictureUrl = "/Uploads/Images/default_image.png",
                            NavBarPicture = "/Uploads/Images/navs/default_nav_image.png",
                            ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png"
                        },
                        new AppUser
                        {
                            UserName = "chris_white",
                            Email = "chris.white@example.com",
                            PictureUrl = "/Uploads/Images/default_image.png",
                            NavBarPicture = "/Uploads/Images/navs/default_nav_image.png",
                            ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png"
                        }
                    };

                    foreach (var user in users)
                    {
                        await userManager.CreateAsync(user, "Hello123");
                    }
                }
                
                if (!context.MediaTypes.Any())
                {
                    List<MediaType> mediaTypes = new List<MediaType>()
                    {
                        new MediaType("Audio"),
                        new MediaType("Video")
                    };

                    context.MediaTypes.AddRange(mediaTypes);
                    context.SaveChanges();
                    
                }

            }
        }
    }
}