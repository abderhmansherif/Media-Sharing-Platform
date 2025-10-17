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
    public class AddRoleViewModel
    {
        [Required(ErrorMessage = "Required")]
        public string RoleName { get; set; }
    }
}