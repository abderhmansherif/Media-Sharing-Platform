using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BeatBox.Areas.Admin.Models;
using BeatBox.Areas.Media.Models;

namespace BeatBox.Models
{
    public class ListeningHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string MediaId { get; set; }
        public DateTime ListenedAt { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        [ForeignKey("MediaId")]
        public MediaElement MediaElement { get; set; }
    }
}