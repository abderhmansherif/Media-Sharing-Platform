using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeatBox.Models
{
    public class MediaOperationResult
    {
        public string PublicId { get; set; }
        public string Url { get; set; }
        public bool result { get; set; }
        public List<string> Errors { get; set; }
    }
}