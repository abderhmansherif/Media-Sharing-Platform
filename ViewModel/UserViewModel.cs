using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatBox.Models;
using Microsoft.AspNetCore.Identity;
using BeatBox.Areas.Admin.Models;


namespace BeatBox.ViewModel
{
    public class UserViewModel
    {
        public IEnumerable<AppUser>? Users { get; set; }
        public IEnumerable<IdentityRole>? Roles { get; set; }
    }
}

   