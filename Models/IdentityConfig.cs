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
            var userManager = serviceProvider.GetService<UserManager<AppUser>>();
            var RoleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetService<AppDbContext>();
            var _webHostEnvironment = serviceProvider.GetService<IWebHostEnvironment>();

            var username = "Abderhman202";
            var Email = "admin202@gmail.com";
            var password = "Abderhman2026";
            var RoleName = "Admin";


            if (await RoleManager.FindByNameAsync(RoleName) == null)
            {
                await RoleManager.CreateAsync(new IdentityRole { Name = RoleName });
            }

            if (await userManager.FindByNameAsync(username) == null)
            {
                AppUser user = new AppUser
                {
                    UserName = username,
                    Email = Email,
                    PictureUrl = "/Uploads/Images/default_image.png",
                    NavBarPicture = "/Uploads/Images/navs/default_nav_image.png",
                    ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png", //D:\.NET MVC\BeatBox\wwwroot\images\profile_images\default_profile_image.png
                };

                var results = await userManager.CreateAsync(user, password);

                if (results.Succeeded)
                    await userManager.AddToRoleAsync(user, RoleName);
            }

            if (!context.Users.Any())
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
                    },
                    new AppUser 
                    { 
                        UserName = "sarah_black", 
                        Email = "sarah.black@example.com", 
                        PictureUrl = "/Uploads/Images/default_image.png", 
                        NavBarPicture = "/Uploads/Images/navs/default_nav_image.png", 
                        ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png" 
                    },
                    new AppUser 
                    { 
                        UserName = "david_green", 
                        Email = "david.green@example.com", 
                        PictureUrl = "/Uploads/Images/default_image.png", 
                        NavBarPicture = "/Uploads/Images/navs/default_nav_image.png", 
                        ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png" 
                    },
                    new AppUser 
                    { 
                        UserName = "laura_blue", 
                        Email = "laura.blue@example.com", 
                        PictureUrl = "/Uploads/Images/default_image.png", 
                        NavBarPicture = "/Uploads/Images/navs/default_nav_image.png", 
                        ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png" 
                    },
                    new AppUser 
                    { 
                        UserName = "matthew_gray", 
                        Email = "matthew.gray@example.com", 
                        PictureUrl = "/Uploads/Images/default_image.png", 
                        NavBarPicture = "/Uploads/Images/navs/default_nav_image.png", 
                        ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png" 
                    },
                    new AppUser 
                    { 
                        UserName = "olivia_pink", 
                        Email = "olivia.pink@example.com", 
                        PictureUrl = "/Uploads/Images/default_image.png", 
                        NavBarPicture = "/Uploads/Images/navs/default_nav_image.png", 
                        ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png" 
                    },
                    new AppUser 
                    { 
                        UserName = "daniel_clark", 
                        Email = "daniel.clark@example.com", 
                        PictureUrl = "/Uploads/Images/default_image.png", 
                        NavBarPicture = "/Uploads/Images/navs/default_nav_image.png", 
                        ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png" 
                    },
                    new AppUser 
                    { 
                        UserName = "sophia_moore", 
                        Email = "sophia.moore@example.com", 
                        PictureUrl = "/Uploads/Images/default_image.png", 
                        NavBarPicture = "/Uploads/Images/navs/default_nav_image.png", 
                        ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png" 
                    },
                    new AppUser 
                    { 
                        UserName = "james_taylor", 
                        Email = "james.taylor@example.com", 
                        PictureUrl = "/Uploads/Images/default_image.png", 
                        NavBarPicture = "/Uploads/Images/navs/default_nav_image.png", 
                        ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png" 
                    },
                    new AppUser 
                    { 
                        UserName = "isabella_wilson", 
                        Email = "isabella.wilson@example.com", 
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

                foreach (var type in mediaTypes)
                {
                    context.MediaTypes.Add(type);
                    context.SaveChanges();
                }
            }


                // context.Medias.Add(media);
                // context.SaveChanges();

                MediaElement mediav = new MediaElement()
                {
                    Title = "John wick3",
                    Descreption = "Action Movie",
                    UserId = "bcfab2b5-5430-4074-b751-33bee895f566",
                    Size = "500MB",
                    MediaTypeId = 2, // "D:\.NET MVC\BeatBox\wwwroot\Uploads\Images\486618439_3553411974960043_3391620105888062273_n.jpg"
                    UploadedAt = DateTime.Today,
                    Url = "/Uploads/Videos/Daredevil-Born-Again(S1E9).mp4",
                    ImageUrl = "/Uploads/Images/486618439_3553411974960043_3391620105888062273_n.jpg",// "D:\.NET MVC\BeatBox\wwwroot\images\464752501_411612945329157_6801942793141381654_n.jpg"
                };

                // context.Medias.Add(mediav);
                // context.SaveChanges();




        }
    }
}