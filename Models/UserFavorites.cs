using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatBox.Areas.Admin.Models;

namespace BeatBox.Areas.Media.Models
{
    public class UserFavorites
    {
        public int Id { get; set; }
        public string MediaId { get; set; } = null!;
        public string UserId { get; set; } = null!;

        public MediaElement Media { get; set; } = null!;
        public AppUser User { get; set; } = null!;
        public DateTime AddedAt { get; set; }

        public UserFavorites(string userId, string mediaId)
        {
            UserId = userId;
            MediaId = mediaId;
            AddedAt = DateTime.Now;
        }
    }
}