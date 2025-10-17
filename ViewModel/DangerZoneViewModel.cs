using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeatBox.ViewModel
{
    public class DangerZoneViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required")]
        public string ConfirmPassword { get; set; }
    }
}