using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeatBox.Models;
using Microsoft.EntityFrameworkCore;
using BeatBox.Areas.Media.Models;
using BeatBox.ViewModel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BeatBox.Services;
using Microsoft.AspNetCore.Identity;
using BeatBox.Areas.Admin.Models;

namespace BeatBox.Areas.Media.Controllers
{

    [Authorize]
    public class MediaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMediaManager _mediaManager;
        private readonly IFileManager _fileManager;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public MediaController(UserManager<AppUser> userManager, AppDbContext context, IMediaManager mediaManager, IFileManager fileManager, IConfiguration configuration)
        {
            _context = context;
            _mediaManager = mediaManager;
            _fileManager = fileManager;
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Explore()
        {
            var medias = await _context.Medias
                        .Include(x => x.User)
                        .Include(x => x.MediaType)
                        .OrderByDescending(x => x.UploadedAt)
                        .ToListAsync();

            if (!medias.Any())
                return View(new List<ExploreViewModel>());

            List<ExploreViewModel> model = medias.Select(x => new ExploreViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Descreption = x.Descreption,
                ImageUrl = x.ImageUrl,
                Url = x.Url,
                UserId = x.UserId,
                Username = x.User?.UserName ?? "Unknown",
                Size = x.Size,
                UploadedAt = x.UploadedAt,
                MediaType = x.MediaType.Name,
                UserImageUrl = x.User?.NavBarPicture ?? "/Uploads/Images/navs/default_nav_image.png",
                MediaTypeId = x.MediaTypeId,
            })
            .ToList();

            return View(model);
        }

     

        [HttpGet]
        public async Task<IActionResult> GetUserMedias(string Id)
        {
            var userExist = await _userManager.FindByIdAsync(Id);

            if (userExist == null)
                return Json(new List<MediaElement>());

            var userMedias = await _context.Medias.Include(x => x.MediaType).Where(m => m.UserId == Id).ToListAsync();

            return Json(userMedias);
        }

        

        [HttpGet]
        public IActionResult GetAllAudios()
        {
            var Audios = _context.Medias
                                    .Include(m => m.MediaType)
                                    .Include(u => u.User)
                                    .Where(m => m.MediaType.Name == "Audio")
                                    .ToList();

            List<MediaViewModel> MediaViewModel = new List<MediaViewModel>();

            foreach (var audio in Audios)
            {
                var mediaViewModel = new MediaViewModel()
                {
                    Id = audio.Id,
                    Title = audio.Title,
                    Descreption = audio.Descreption,
                    Url = audio.Url,
                    UploadedAt = audio.UploadedAt,
                    Size = audio.Size,
                    MediaTypeId = audio.MediaType.Id,
                    ImageUrl = audio.ImageUrl,
                    UserId = audio.UserId,
                    User = audio.User,
                };

                MediaViewModel.Add(mediaViewModel);
            }

            return View("Audios", MediaViewModel);
        }

        [HttpGet]
        public IActionResult GetAllVideos()
        {
            var Videos = _context.Medias
                                    .Include(m => m.MediaType)
                                    .Include(x => x.User)
                                    .Where(m => m.MediaType.Name == "Video")
                                    .ToList();

            List<MediaViewModel> MediaViewModel = new List<MediaViewModel>();

            foreach (var video in Videos)
            {
                var mediaViewModel = new MediaViewModel()
                {
                    Id = video.Id,
                    Title = video.Title,
                    Descreption = video.Descreption,
                    Url = video.Url,
                    UploadedAt = video.UploadedAt,
                    Size = video.Size,
                    MediaTypeId = video.MediaType.Id,
                    ImageUrl = video.ImageUrl,
                    UserId = video.UserId,
                    User = video.User,
                };

                MediaViewModel.Add(mediaViewModel);
            }

            return View("Videos", MediaViewModel);

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create(string Id)
        {
            var userExist = await _userManager.FindByIdAsync(Id);

            if (userExist == null)
                return NotFound();

            var UserIdRequest = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            if (userExist.Id != UserIdRequest)
                return NotFound();

            MediaViewModel model = new MediaViewModel()
            {
                UserId = Id
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(2_147_483_648)] // 2GB
        [RequestFormLimits(MultipartBodyLengthLimit = 2_147_483_648)]
        public async Task<IActionResult> Create(MediaViewModel mediaViewModel)
        {
            if (ModelState.IsValid)
            {
                mediaViewModel.MediaTypes = _context.MediaTypes.ToList();

                var userExist = await _userManager.FindByIdAsync(mediaViewModel.UserId);

                if (userExist == null)
                    return NotFound();

                // Validate Image File if exists
                if (mediaViewModel.ImageFile != null)
                {
                    var isValidImage = _mediaManager.ImageValidationFilter(mediaViewModel.ImageFile);
                    if (!isValidImage)
                    {
                        ModelState.AddModelError("ImageFile", "Not valid image");
                        return View(mediaViewModel);
                    }
                }

                // Validate Media File
                bool isValidFile;
                if (mediaViewModel.MediaFile.ContentType.StartsWith("video/"))
                {
                    isValidFile = _mediaManager.VideoValidationFilter(mediaViewModel.MediaFile);
                    mediaViewModel.MediaTypeId = _context.MediaTypes.FirstOrDefault(x => x.Name == "Video").Id;
                }
                else if (mediaViewModel.MediaFile.ContentType.StartsWith("audio/"))
                {
                    isValidFile = _mediaManager.AudioValidationFilter(mediaViewModel.MediaFile);
                    mediaViewModel.MediaTypeId = _context.MediaTypes.FirstOrDefault(x => x.Name == "Audio").Id;
                }
                else
                {
                    isValidFile = false;
                }

                if (!isValidFile)
                {
                    ModelState.AddModelError("MediaFile", "Not valid Media");
                    return View(mediaViewModel);
                }

                MediaOperationResult imageUploadResult = null;
                MediaOperationResult mediaUploadResult = null;

                // Upload files based on configuration
                if (_configuration.GetValue<bool>("UseCloudinary"))
                {
                    // Upload to Cloudinary
                    if (mediaViewModel.ImageFile != null)
                    {
                        imageUploadResult = await _mediaManager.UploadImageAsync(mediaViewModel.ImageFile);
                    }

                    if (mediaViewModel.MediaFile.ContentType.StartsWith("video/"))
                    {
                        mediaUploadResult = await _mediaManager.UploadVideoAsync(mediaViewModel.MediaFile);
                    }
                    else if (mediaViewModel.MediaFile.ContentType.StartsWith("audio/"))
                    {
                        mediaUploadResult = await _mediaManager.UploadAudioAsync(mediaViewModel.MediaFile);
                    }
                }
                else
                {
                    // Upload to local server
                    if (mediaViewModel.ImageFile != null)
                    {
                        imageUploadResult = await _mediaManager.UploadImageToServerAsync(mediaViewModel.ImageFile);
                    }

                    if (mediaViewModel.MediaFile.ContentType.StartsWith("video/"))
                    {
                        mediaUploadResult = await _mediaManager.UploadVideoToServerAsync(mediaViewModel.MediaFile);
                    }
                    else if (mediaViewModel.MediaFile.ContentType.StartsWith("audio/"))
                    {
                        mediaUploadResult = await _mediaManager.UploadAudioToServerAsync(mediaViewModel.MediaFile);
                    }
                }

                // Check upload results
                if ((mediaViewModel.ImageFile != null && !imageUploadResult.result) || !mediaUploadResult.result)
                {
                    if (mediaViewModel.ImageFile != null && !imageUploadResult.result)
                    {
                        ModelState.AddModelError("ImageFile", string.Join(", ", imageUploadResult.Errors));
                    }
                    if (!mediaUploadResult.result)
                    {
                        ModelState.AddModelError("MediaFile", string.Join(", ", mediaUploadResult.Errors));
                    }
                    return View(mediaViewModel);
                }

                // Store upload records in database
                if (_configuration.GetValue<bool>("useCloudinary"))
                {
                    if (mediaViewModel.ImageFile != null)
                    {
                        var isImageStored = await _fileManager.CreateCloudinaryUploadAsync(new CloudinaryUploads()
                        {
                            PublicId = imageUploadResult.PublicId,
                            Url = imageUploadResult.Url
                        });

                        if (!isImageStored.Result)
                        {
                            ModelState.AddModelError("ImageFile", string.Join(", ", isImageStored.Errors));
                            return View(mediaViewModel);
                        }
                    }

                    var isMediaStored = await _fileManager.CreateCloudinaryUploadAsync(new CloudinaryUploads()
                    {
                        PublicId = mediaUploadResult.PublicId,
                        Url = mediaUploadResult.Url
                    });

                    if (!isMediaStored.Result)
                    {
                        ModelState.AddModelError("MediaFile", string.Join(", ", isMediaStored.Errors));
                        return View(mediaViewModel);
                    }
                }

                // Create media entity
                MediaElement mediaFile = new MediaElement()
                {
                    Title = mediaViewModel.Title,
                    Descreption = mediaViewModel.Descreption,
                    MediaTypeId = mediaViewModel.MediaTypeId,
                    ImageUrl = mediaViewModel.ImageFile != null ? imageUploadResult.Url : null,
                    Url = mediaUploadResult.Url,
                    Size = _mediaManager.GetMediaSize(mediaViewModel.MediaFile).ToString(),
                    UploadedAt = DateTime.Now,
                    UserId = mediaViewModel.UserId,
                };

                await _context.Medias.AddAsync(mediaFile);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(mediaViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ToggleFavorite(string mediaId)
        {
            var media = _context.Medias.FirstOrDefault(m => m.Id == mediaId);
            if (media == null)
                return Json(false);

            string userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var existing = _context.UserFavorites.FirstOrDefault(x => x.MediaId == mediaId && x.UserId == userId);

            bool isFavorite;
            if (existing is null)
            {
                _context.UserFavorites.Add(new UserFavorites(userId, mediaId));

                _context.SaveChanges();
                isFavorite = true;
            }
            else
            {
                _context.UserFavorites.Remove(existing);
                _context.SaveChanges();
                isFavorite = false;
            }

            return Json(isFavorite);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RecordListening(string mediaId)
        {
            string userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var isExist = await _context.ListeningHistory
                                        .FirstOrDefaultAsync(x => x.MediaId == mediaId && x.UserId == userId);

            if (mediaId == null)
                return Ok(new { success = false, alreadyExist = false });

            if (isExist != null)
            {
                isExist.ListenedAt = DateTime.Now;
                _context.ListeningHistory.Update(isExist);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, alreadyExist = false });
            }

            var record = new ListeningHistory()
            {
                MediaId = mediaId,
                UserId = userId,
                ListenedAt = DateTime.Now,
            };

            await _context.ListeningHistory.AddAsync(record);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, alreadyExist = false });
        }


    }
}


