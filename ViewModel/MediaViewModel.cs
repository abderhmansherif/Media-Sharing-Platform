using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BeatBox.Areas.Admin.Models;
using BeatBox.Areas.Media.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeatBox.ViewModel
{
    public class MediaViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(50, ErrorMessage = "Must be less than 50 charchter")]
        [MinLength(1, ErrorMessage = "At least 1 charechter")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Required")]
        [MaxLength(250, ErrorMessage = "Must be less than 250 charchter")]
        [MinLength(1, ErrorMessage = "At least 1 charechter")]
        public string Descreption { get; set; }

        public string? Size { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime UploadedAt { get; set; }
        public string? Url { get; set; }

        [Required(ErrorMessage = "Choose Media Type")]
        public int MediaTypeId { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "Required")]
        public IFormFile? ImageFile { get; set; }


        [Required(ErrorMessage = "Required")]
        public IFormFile MediaFile { get; set; }

        public List<MediaType>? MediaTypes { get; set; }

        public AppUser? User { get; set; }
        

    }
}