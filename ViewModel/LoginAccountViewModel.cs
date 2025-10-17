using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeatBox.ViewModel
{
    public class LoginAccountViewModel
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Not valid Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 20, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)]
        public string Password { get; set; } = null!;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}