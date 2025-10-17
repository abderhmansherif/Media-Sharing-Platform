using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeatBox.ViewModel
{
    public class ExploreViewModel
    {
        public string? Id { get; set; }
        public string Title { get; set; }
        public string Descreption { get; set; }
        public string? Size { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime UploadedAt { get; set; }
        public string? Url { get; set; }
        public int MediaTypeId { get; set; }
        public string MediaType { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string UserImageUrl { get; set; }
    }
}