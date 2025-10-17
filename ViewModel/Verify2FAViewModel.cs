using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeatBox.ViewModel
{
    public class Verify2FAViewModel
    {
        [Required]
        public string Id { get; set; }
        
        [Required]
        public string Code { get; set; }
    }
}