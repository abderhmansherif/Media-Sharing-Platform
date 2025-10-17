using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BeatBox.Models;
using Microsoft.AspNetCore.Identity;
using BeatBox.Areas.Admin.Models;
using BeatBox.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BeatBox.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly  AppDbContext context;
    private readonly UserManager<AppUser> userManager;

    public HomeController(ILogger<HomeController> logger, AppDbContext context, UserManager<AppUser> userManager)
    {
        _logger = logger;
        this.context = context;
        this.userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var AudioMedias = await context.Medias
                .Include(x => x.User)
                .Where(x => x.MediaType.Name == "Audio")
                .OrderByDescending(x => x.UploadedAt)
                .Take(3)
                .Select(x => new MediaViewModel()
                {
                    Id = x.Id,
                    Url = x.Url,
                    ImageUrl = x.ImageUrl,
                    Size = x.Size,
                    Title = x.Title,
                    Descreption = x.Descreption,
                    UserId = x.UserId,
                    User = x.User,
                })
                .ToListAsync();

            
        var VideoMedias = await context.Medias
                .Include(x => x.User)
                .Where(x => x.MediaType.Name == "Video")
                .OrderByDescending(x => x.UploadedAt)
                .Take(3)
                .Select(x => new MediaViewModel()
                {
                    Id = x.Id,
                    Url = x.Url,
                    ImageUrl = x.ImageUrl,
                    Size = x.Size,
                    Title = x.Title,
                    Descreption = x.Descreption,
                    UserId = x.UserId,
                    User = x.User,
                })
                .ToListAsync();

        var TotalUploads = await context.Medias.CountAsync();

        var TotalUsers = await context.Users.CountAsync();

       

        var model = new HomeViewModel()
        {
            RecentAudioMedias = AudioMedias,
            TotalUsers = TotalUsers,
            TotalUploads = TotalUploads,
            RecentVideoMedias = VideoMedias,
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
