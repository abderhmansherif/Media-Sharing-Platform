using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatBox.Models;
using BeatBox.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BeatBox.Areas.Admin.Models;
using BeatBox.Services;
using static System.Net.Mime.MediaTypeNames;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

namespace BeatBox.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly EmailService _emailService;
        public AccountController(UserManager<AppUser> userManager,
                                SignInManager<AppUser> signInManager,
                                IWebHostEnvironment webHostEnvironment,
                                EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpGet]
        public IActionResult ConfirmYourEmail() => View();


        [HttpPost]
        public async Task<IActionResult> Register(RegisterAccountViewModel registerAccountViewModel)
        {
            if (ModelState.IsValid)
            {
                // checks if already registerd
                var userExist = await _userManager.FindByEmailAsync(registerAccountViewModel.Email);

                if (userExist != null)
                {
                    ModelState.AddModelError("Email", AppMessages.EmailAlreadyRegistered);
                    return View(registerAccountViewModel);
                }

                // store the user
                var user = new AppUser()
                {
                    UserName = registerAccountViewModel.Username,
                    Email = registerAccountViewModel.Email,
                    PictureUrl = "/images/default_image.png",
                    NavBarPicture = "/Uploads/Images/navs/default_nav_image.png",
                    ProfilePicture = "/Uploads/Images/profiles/default_profile_image.png",
                    EmailConfirmed = false
                };

                try
                {
                    var results = await _userManager.CreateAsync(user, registerAccountViewModel.Password);

                    if (results.Succeeded)
                    {
                        return RedirectToAction("ResendConfirmEmail", new { id = user.Id });
                    }

                    ModelState.AddModelError("", String.Join(",", results.Errors.Select(x => x.Description)));
                    return View(registerAccountViewModel);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", AppMessages.PasswordError);
                }
            }
            return View(registerAccountViewModel);
        }

        [HttpGet]
        public IActionResult EmailConfirmedSuccessfully() => View();


        [HttpGet]
        public IActionResult EmailConfirmationFailed() => View();


        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string Id, string token)
        {
            if (Id == null || token == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);


            if (result.Succeeded)
            {
                user.EmailConfirmed = true;

                var resultUpdates = await _userManager.UpdateAsync(user);

                if (resultUpdates.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("EmailConfirmedSuccessfully", "Account");
                }

            }

            return RedirectToAction("EmailConfirmationFailed", "Account");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);

                    if (checkPassword)
                    {
                        if (!user.EmailConfirmed)
                            return RedirectToAction("ResendConfirmEmail", "Account", new { Id = user.Id });

                        if (user.TwoFactorEnabled)
                            return RedirectToAction(actionName: "Verify2FA", controllerName: "Account", new { id = user.Id });

                        await _signInManager.SignInAsync(user, model.RememberMe);

                        TempData["message"] = AppMessages.LoginSuccess;

                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("Password", AppMessages.InvalidLogin);
            }
            return View(model);
        }

        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpGet]
        public IActionResult FaildSendingEmail() => View();

        [HttpGet]
        public IActionResult SuccessSendingEmail() => View();

        [HttpGet]
        public IActionResult ResetPasswordSuccess() => View();


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("email", "this email does not exist");
                return View();
            }

            // creating forgotpassword url

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var CallBack_Url = Request.Scheme + "://" + Request.Host + Url.Action(action: "ResetPassword", controller: "Account", new { id = user.Id, token = token });

            var emailBodyHtml = EmailBodies.ForgotPasswordHtmlContent.Replace("{{ResetLink}}", CallBack_Url).Replace("{{UserName}}", user.UserName);

            var emailBodyPlainText = EmailBodies.ForgotPasswordPlainTextContent.Replace("{{ResetLink}}", CallBack_Url).Replace("{{UserName}}", user.UserName);

            var isSended = await _emailService.SendEmail(user.Email, "Reset Password", emailBodyHtml, emailBodyPlainText);

            if (!isSended)
                return RedirectToAction("FaildSendingEmail", "Account");

            return RedirectToAction("SuccessSendingEmail");

        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id, string token)
        {
            var userExist = await _userManager.FindByIdAsync(id);

            if (userExist == null)
                return NotFound();

            var model = new ResetPasswordViewModel()
            {
                Id = id,
                Token = token
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ResendConfirmEmail(string Id)
        {
            if (Id == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
                return NotFound();

            if (user.EmailConfirmed)
                return NotFound();

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmEmailUrl = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", "Account", new { Id = user.Id, token = token });

            var emailbodyHtml = EmailBodies.ConfirmEmailHtmlBody.Replace("{{ResetLink}}", confirmEmailUrl).Replace("{{UserName}}", user.UserName);

            var emailBodyPlainText = EmailBodies.ConfirmEmailPlainText.Replace("{{ResetLink}}", confirmEmailUrl).Replace("{{UserName}}", user.UserName);

            var isSended = await _emailService.SendEmail(user.Email, "Email Confirmation", emailbodyHtml, emailBodyPlainText);

            if (isSended)
                return RedirectToAction("ConfirmYourEmail", "Account");

            return RedirectToAction("FaildSendingEmail", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordViewModel);

            var user = await _userManager.FindByIdAsync(resetPasswordViewModel.Id);

            if (user == null)
            {
                ModelState.AddModelError("", "User Not Found");
                return View(resetPasswordViewModel);
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Token, resetPasswordViewModel.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(resetPasswordViewModel);
            }

            return RedirectToAction("ResetPasswordSuccess");
        }

        [HttpGet]
        public async Task<IActionResult> Verify2FA(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

            var emailBodyHtml = EmailBodies.TwoFactorOtpHtmlBody.Replace("{{UserName}}", user.UserName).Replace("{{OTPCode}}", code);
            var emailBodyPlainText = EmailBodies.TwoFactorOtpPlainText.Replace("{{UserName}}", user.UserName).Replace("{{OTPCode}}", code);

            var isSended = await _emailService.SendEmail(
                to: user.Email,
                "Your BeatBox Verification Code",
                emailBodyHtml,
                emailBodyPlainText
            );

            if (isSended)
                return View(new Verify2FAViewModel() { Id = user.Id });

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verify2FA(Verify2FAViewModel verify2FAViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(verify2FAViewModel.Id);

                if (user == null)
                    return NotFound();

                var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", verify2FAViewModel.Code);

                if (isValid)
                {
                    await _signInManager.SignInAsync(user, true);
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Verify2FA", new { id = verify2FAViewModel.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Resend2FA(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();


            var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

            var emailBodyHtml = EmailBodies.TwoFactorOtpHtmlBody.Replace("{{UserName}}", user.UserName).Replace("{{OTPCode}}", code);
            var emailBodyPlainText = EmailBodies.TwoFactorOtpPlainText.Replace("{{UserName}}", user.UserName).Replace("{{OTPCode}}", code);

            var isSended = await _emailService.SendEmail(
                to: user.Email,
                "Your BeatBox Verification Code",
                emailBodyHtml,
                emailBodyPlainText
            );

            if (isSended)
                return RedirectToAction(actionName: "Verify2FA", controllerName: "Account" , new { Id = user.Id });


            return RedirectToAction("Login", "Account");
        }


    }
}