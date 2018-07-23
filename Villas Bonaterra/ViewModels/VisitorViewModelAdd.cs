using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VillasBonaterra.ViewModels
{
    
    public class RegisterVisitorModel
    {   
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "MidleName")]
        public string MidleName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "BirthDay")]
        public string BirthDay { get; set; }
        
        [Required]
        [Display(Name = "Photo")]
        public string Photo { get; set; }
    }

    public class LastVisitorModel
    {


        [Required]
        [Display(Name = "PIN")]
        public string PIN { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "MidleName")]
        public string MidleName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "BirthDay")]
        public DateTime BirthDay { get; set; }

        [Required]
        [Display(Name = "LastAccess")]
        public DateTime? LastAccess { get; set; }

        [Required]
        [Display(Name = "Photo")]
        public string Photo { get; set; }

        [Display(Name = "Plate")]
        public string Plate { get; set; }
    }
}