using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BeatBox.Areas.Admin.Models;
using BeatBox.Models;
using BeatBox.Services;
using BeatBox.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using CloudinaryDotNet;

namespace BeatBox.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediaManager _mediaManager;
        private readonly IWebHostEnvironment _environment;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly EmailService _emailService;


        public SettingsController(UserManager<AppUser> userManager,
                                AppDbContext context, IMediaManager mediaManager,
                                    IWebHostEnvironment environment,
                                    SignInManager<AppUser> signInManager,
                                    EmailService emailService)
        {
            _userManager = userManager;
            _context = context;
            _mediaManager = mediaManager;
            _environment = environment;
            _signInManager = signInManager;
            _emailService = emailService;
        }


        [HttpGet]
        public IActionResult Index() => View();


        public async Task<IActionResult> ChangePasswordPartial()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new ChangePasswordViewModel()
            {
                UserId = user.Id
            };

            return PartialView("_ChangePasswordPartial", model);
        }

        public async Task<IActionResult> TwoFactorPartial()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new TwoFactorViewModel()
            {
                TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
            };

            return PartialView("_TwoFactor", model);
        }


        public async Task<IActionResult> AccountSettings()
        {
            var user = await _userManager.GetUserAsync(User);

            var AccountSettings = new AccountSettingsViewModel()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                ProfileImageUrl = user.ProfilePicture,
            };
            return PartialView("_AccountSettings", AccountSettings);
        }


        public async Task<IActionResult> DangerZoneSettings()
        {
            var user = await _userManager.GetUserAsync(User);

            var DangerZoneSettings = new DangerZoneViewModel()
            {
                Id = user.Id,
            };

            return PartialView("_DangerZone", DangerZoneSettings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AccountSettingsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userExist = await _userManager.FindByIdAsync(model.Id);

            if (userExist == null)
                return NotFound();

            string? profileImageUrl = null;
            string? navImageUrl = null;
            string? OriginalImageUrl = null;

            if (model.ImageFile != null)
            {
                var isValidFile = _mediaManager.ImageValidationFilter(model.ImageFile);
                if (!isValidFile)
                {
                    ModelState.AddModelError("ImageFile", "Invalid file format");
                    return View(model);
                }

                var isStored = await _mediaManager.UploadImageToServerAsync(model.ImageFile);

                if (!isStored.result)
                {
                    ModelState.AddModelError("", "Server Error Faild To Upload the image");
                    return View(model);
                }

                OriginalImageUrl = isStored.Url;

                using var stream = model.ImageFile.OpenReadStream();
                using var image = await Image.LoadAsync(stream);

                // Nav 50x50
                using (var smallImage = image.Clone(x => x.Resize(new ResizeOptions { Mode = ResizeMode.Crop, Size = new Size(50, 50) })))
                {
                    var fileName = $"nav_{model.Id}_{Guid.NewGuid()}.png";
                    var savePath = Path.Combine(_environment.WebRootPath, "Uploads", "Images", "navs", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
                    navImageUrl = $"/Uploads/Images/navs/{fileName}";
                    await smallImage.SaveAsync(savePath, new PngEncoder());
                }

                // Profile 300x300
                using (var largeImage = image.Clone(x => x.Resize(new ResizeOptions { Mode = ResizeMode.Crop, Size = new Size(300, 300) })))
                {
                    var fileName = $"profile_{model.Id}_{Guid.NewGuid()}.png";
                    var savePath = Path.Combine(_environment.WebRootPath, "Uploads", "Images", "profiles", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
                    profileImageUrl = $"/Uploads/Images/profiles/{fileName}";
                    await largeImage.SaveAsync(savePath, new PngEncoder());
                }
            }

            var emailOwner = await _userManager.FindByEmailAsync(model.Email);

            if (emailOwner != null && emailOwner.Id != model.Id)
            {
                ModelState.AddModelError("Email", "This Email is already in use");
                return View(model);
            }

            userExist.NavBarPicture = navImageUrl ?? "/Uploads/Images/navs/default_nav_image.png";
            userExist.ProfilePicture = profileImageUrl ?? "/Uploads/Images/profiles/default_profile_image.png";
            userExist.UserName = model.Username;
            userExist.Email = model.Email;
            userExist.PictureUrl = OriginalImageUrl ?? "/images/profile_images/default_profile_image.png";

            var res = await _userManager.SetEmailAsync(userExist, model.Email);

            if (!res.Succeeded)
            {
                ModelState.AddModelError("", "Faild to update profile");
                return View(model);
            }

            var result = await _userManager.UpdateAsync(userExist);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", String.Join(",", result.Errors.Select(x => x.Description)));
                return View(model);
            }

            return RedirectToAction(actionName: "Index", controllerName: "Settings");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProfilePicture(string Id)
        {
            var userExist = await _userManager.FindByIdAsync(Id);

            if (userExist == null)
                return NotFound();

            userExist.PictureUrl = "/images/profile_images/default_profile_image.png";
            userExist.NavBarPicture = "/Uploads/Images/navs/default_nav_image.png";
            userExist.ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png";

            var result = await _userManager.UpdateAsync(userExist);

            if (!result.Succeeded)
                return Json(new { success = false, message = "Failed to remove profile picture" });

            return RedirectToAction(actionName: "Index", controllerName: "Settings");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(DangerZoneViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                if (user == null)
                    return NotFound();

                var isCorrectPassword = await _userManager.CheckPasswordAsync(user, model.ConfirmPassword);

                if (!isCorrectPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "InCorrect Passowrd");
                    return RedirectToAction("Index", "Settings");
                }


                // Delete the User
                var IsDeleted = await _userManager.DeleteAsync(user);

                if (!IsDeleted.Succeeded)
                {
                    ModelState.AddModelError("", "Server Error try agin later");
                    return RedirectToAction("Index", "Settings");
                }

                // Delete Listening History
                var historyList = _context.ListeningHistory.Where(x => x.UserId == model.Id).ToList();

                _context.ListeningHistory.RemoveRange(historyList);
                await _context.SaveChangesAsync();

                // Delete Favorites
                var favoritesList = _context.UserFavorites.Where(x => x.UserId == model.Id);

                _context.UserFavorites.RemoveRange(favoritesList);
                await _context.SaveChangesAsync();

                // Delete the Cookie
                await _signInManager.SignOutAsync();

                return RedirectToAction(actionName: "Index", controllerName: "Home");
            }
            return RedirectToAction("Index", "Settings");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var userExist = await _userManager.FindByIdAsync(changePasswordViewModel.UserId);

                if (userExist == null)
                    return NotFound();

                //Checking Password 
                var istrue = await _userManager.CheckPasswordAsync(userExist, changePasswordViewModel.CurrentPassword);
                if (!istrue)
                {
                    ModelState.AddModelError("CurrentPassword", "Invalid Password");
                    return RedirectToAction(nameof(Index));
                }

                var results = await _userManager.ChangePasswordAsync(userExist, changePasswordViewModel.CurrentPassword, changePasswordViewModel.NewPassword);

                if (results.Succeeded)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction(actionName: "Login", controllerName: "Account");
                }

                ModelState.AddModelError("", String.Join(",", results.Errors.Select(x => x.Description)));
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(TwoFactorViewModel twoFactorViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                    return NotFound();

                user.HasAuthenticator = true;
                user.TwoFactorEnabled = true;

                await _userManager.UpdateAsync(user);

                // Sending an email 

                var emailBodyHtml = EmailBodies.TwoFactorEnabledHtmlBody.Replace("{{UserName}}", user.UserName);
                var emailBodyPlainText = EmailBodies.TwoFactorEnabledPlainText.Replace("{{UserName}}", user.UserName);

                var isSended = await _emailService.SendEmail(
                    to: user.Email,
                    subject: "2-Factor Authentication Enabled",
                    emailBodyHtml: emailBodyHtml,
                    emailBodyPlainText: emailBodyPlainText
                );

                if (isSended)
                    return RedirectToAction("Index", "Home");

                return RedirectToAction(actionName: nameof(Index));

            }
            return RedirectToAction(actionName: nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable2FA(TwoFactorViewModel twoFactorViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (User is null)
                    return NotFound();

                if (user.TwoFactorEnabled)
                {
                    user.TwoFactorEnabled = false;
                    user.HasAuthenticator = false;

                    await _userManager.UpdateAsync(user);

                    //Sending Email
                    var emailBodyHtml = EmailBodies.TwoFactorDisabledHtmlBody.Replace("{{UserName}}", user.UserName);
                    var emailBodyPlainText = EmailBodies.TwoFactorDisabledPlainText.Replace("{{UserName}}", user.UserName);

                    var isSended = await _emailService.SendEmail(
                        to: user.Email,
                        subject: "2-Factor Authentication Disabled",
                        emailBodyHtml: emailBodyHtml,
                        emailBodyPlainText: emailBodyPlainText
                    );

                    if (isSended)
                        return RedirectToAction("Index", "Home");

                    return RedirectToAction(actionName: nameof(Index));


                }
            }
            return RedirectToAction(nameof(Index));
        } 
    }
}