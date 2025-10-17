using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeatBox.ViewModel
{
    public class HomeViewModel
    {
        public List<MediaViewModel> RecentAudioMedias { get; set; }
        public List<MediaViewModel> RecentVideoMedias { get; set; }

        public int TotalUploads { get; set; }
        public int TotalUsers { get; set; }
    }
}