using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeatBox.ViewModel
{
    public class TwoFactorViewModel
    {
        [Required]
        public string Provider { get; set; }
        public bool TwoFactorEnabled { get; set; } = false;
    }
}