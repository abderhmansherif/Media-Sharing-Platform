using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BeatBox.Areas.Media.Models;
using Microsoft.AspNetCore.Identity;

namespace BeatBox.Areas.Admin.Models
{
    public class AppUser : IdentityUser
    {
        public IList<UserFavorites> UserFavorites { get; set; }

        [NotMapped]
        public IList<string>? RoleNames { get; set; }

        [NotMapped]
        public IList<MediaElement> Medias { get; set; }
        public string PictureUrl { get; set; }
        public string? NavBarPicture { get; set; }
        public string? ProfilePicture { get; set; }
        public bool HasAuthenticator { get; set; }
        public string? Bio { get; set; }
    }
}