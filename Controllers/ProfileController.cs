using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BeatBox.Areas.Admin.Models;
using BeatBox.Areas.Media.Models;
using BeatBox.Models;
using BeatBox.Services;
using BeatBox.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeatBox.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user is null)
                return NotFound();


            ProfileViewModel profileViewModel = new ProfileViewModel()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                PictureUrl = user.ProfilePicture,
                Bio = user.Bio
            };
            return View(profileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string userId, string bio)
        {
            if (bio == null)
                return View();

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return View();

            user.Bio = bio;
            await _userManager.UpdateAsync(user);


            return RedirectToAction(actionName: "Index", controllerName: "Profile", new { Id = userId });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserFavoriteVideos()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userFavoriteVideos = await _context.UserFavorites
                .Include(x => x.Media)
                .ThenInclude(x => x.MediaType)
                .Where(x => x.UserId == userId && x.Media.MediaType.Name == "Video")
                .OrderByDescending(x => x.AddedAt)
                .Select(x => new
                {
                    Id = x.Media.Id,
                    title = x.Media.Title,
                    thumbnailUrl = x.Media.ImageUrl,
                    url = x.Media.Url,
                    description = x.Media.Descreption,
                    uploadedAt = x.Media.UploadedAt,
                })
                .ToListAsync();

            return Json(userFavoriteVideos);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserFavoriteAudios()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userFavoriteAudios = await _context.UserFavorites
                .Include(x => x.Media)
                .ThenInclude(x => x.MediaType)
                .Where(x => x.UserId == userId && x.Media.MediaType.Name == "Audio")
                .OrderByDescending(x => x.AddedAt)
                .Select(x => new
                {
                    Id = x.Media.Id,
                    title = x.Media.Title,
                    thumbnailUrl = x.Media.ImageUrl,
                    url = x.Media.Url,
                    description = x.Media.Descreption,
                    uploadedAt = x.Media.UploadedAt,

                })
                .ToListAsync();

            return Json(userFavoriteAudios);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetListeningHistoryCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var count = await _context.ListeningHistory
                .Where(x => x.UserId == userId)
                .CountAsync();

            return Json(count);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserFavoritesCount()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;


            var count = await _context.UserFavorites
                .Where(x => x.UserId == userId)
                .CountAsync();

            return Json(count);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ListeningHistory(int page, int pageSize)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var history = await _context.ListeningHistory
                .Include(x => x.MediaElement)
                .ThenInclude(x => x.MediaType)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.ListenedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new
                {
                    id = x.MediaElement.Id,
                    title = x.MediaElement.Title,
                    thumbnailUrl = x.MediaElement.ImageUrl,
                    url = x.MediaElement.Url,
                    type = x.MediaElement.MediaType.Name,
                    description = x.MediaElement.Descreption,
                    uploadedAt = x.MediaElement.UploadedAt
                })
                .ToListAsync();

            return Json(history);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserMedias(string Id)
        {
            var userExist = await _userManager.FindByIdAsync(Id);

            if (userExist == null)
                return Json(new List<MediaElement>());

            var userMedias = await _context.Medias
                                .Include(x => x.MediaType)
                                .Where(m => m.UserId == Id)
                                .OrderByDescending(m => m.UploadedAt)
                                .Select(x => new
                                {
                                    id = x.Id,
                                    title = x.Title,
                                    descreption = x.Descreption,
                                    imageurl = x.ImageUrl,
                                    url = x.Url,
                                    addedAt = x.UploadedAt,
                                    size = x.Size,
                                    mediaType = x.MediaType.Name,
                                })
                                .ToListAsync();


            return Json(userMedias);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAudios(string Id)
        {
            var userExist = await _userManager.FindByIdAsync(Id);

            if (userExist == null)
                return Json(new List<MediaElement>());

            var userMedias = await _context.Medias
                                .Include(x => x.MediaType)
                                .Where(m => m.UserId == Id && m.MediaType.Name == "Audio")
                                .OrderByDescending(m => m.UploadedAt)
                                .Select(x => new
                                {
                                    id = x.Id,
                                    title = x.Title,
                                    descreption = x.Descreption,
                                    imageurl = x.ImageUrl,
                                    url = x.Url,
                                    addedAt = x.UploadedAt,
                                    size = x.Size,
                                    mediaType = x.MediaType.Name,
                                })
                                .ToListAsync();

            return Json(userMedias);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserVideos(string Id)
        {
            var userExist = await _userManager.FindByIdAsync(Id);

            if (userExist == null)
                return Json(new List<MediaElement>());

            var userMedias = await _context.Medias
                                    .Include(x => x.MediaType)
                                    .Where(m => m.UserId == Id && m.MediaType.Name == "Video")
                                    .OrderByDescending(x => x.UploadedAt)
                                    .Select(x => new
                                    {
                                        id = x.Id,
                                        title = x.Title,
                                        descreption = x.Descreption,
                                        imageurl = x.ImageUrl,
                                        url = x.Url,
                                        addedAt = x.UploadedAt,
                                        size = x.Size,
                                        mediaType = x.MediaType.Name,
                                    })
                                    .ToListAsync();

            return Json(userMedias);
        }

        [HttpGet] //Profile/GetOverView/id
        public async Task<IActionResult> GetOverView(string Id)
        {
            var userExist = await _userManager.FindByIdAsync(Id);

            if (userExist == null)
                return NotFound();

            var profileViewModel = new ProfileViewModel()
            {
                Id = userExist.Id,
                Username = userExist.UserName,
                Email = userExist.Email,
                PictureUrl = userExist.PictureUrl,
                Bio = userExist.Bio,
            };

            return View(profileViewModel);
        }

        [HttpDelete] // profile/DeleteMedia?mediaId=id
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMedia(string mediaId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Json(new { success = false });

            var userId = user.Id;

            var mediaExist = await _context.Medias.FirstOrDefaultAsync(x => x.Id == mediaId && x.UserId == userId);

            if (mediaExist == null)
                return Json(new { success = false });

            _context.Medias.Remove(mediaExist);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }




        
    }
}