using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeatBox.ViewModel
{
    public class RegisterAccountViewModel
    {
        [Required(ErrorMessage = "Required")]
        [MaxLength(30, ErrorMessage = "Username should be less than 30 letters")]
        [MinLength(4, ErrorMessage = "Username must be at least 4 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Not valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 20, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Does not match")]
        public string ConfirmPassword { get; set; } = null!;
    }
}