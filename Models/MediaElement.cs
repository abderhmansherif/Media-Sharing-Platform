using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BeatBox.Models;
using System.Threading.Tasks;
using BeatBox.Areas.Admin.Models;

namespace BeatBox.Areas.Media.Models
{
    public class MediaElement
    {
        public string Id { get; set; }
        public string Title { get; set; }
    
        [ForeignKey("MediaType")]
        public int MediaTypeId { get; set; }
        public string? UserId { get; set; }
        public string? Descreption { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public string Size { get; set; }
        public DateTime UploadedAt { get; set; }
        public MediaType MediaType { get; set; }
        public IList<UserFavorites>? UserFavorites { get; set; }

        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

        public MediaElement()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}