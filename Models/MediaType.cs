using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeatBox.Areas.Media.Models
{
    public class MediaType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public MediaType(string name)
        {
            Name = name;
        }

    }
}