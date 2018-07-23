using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VillasBonaterra.ViewModels
{
    public class UserLoginAPPViewModel
    {
        [Required]
        [Display(Name = "user")]
        public string user { get; set; }

        [Required]
        [Display(Name = "password")]
        public string password { get; set; }

    }
}