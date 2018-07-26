using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VillasBonaterra.ViewModels
{
    public class ResidencenceAddViewModel
    {
        [Required]
        [Display(Name = "Street")]
        public Guid Street { get; set; }

        [Required]
        [Display(Name = "No_Calle")]
        public string No_Calle { get; set; }

        [Required]
        [Display(Name = "OwnerId")]
        public Guid OwnerId { get; set; }
    }
}