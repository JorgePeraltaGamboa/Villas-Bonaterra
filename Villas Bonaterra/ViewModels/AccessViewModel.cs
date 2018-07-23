using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VillasBonaterra.ViewModels
{
    public class AccessViewModel
    {
        [Required]
        [Display(Name = "Date")]
        public DateTime? Date { get; set; }

        [Display(Name = "Date")]
        public DateTime? DateOut { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Number")]
        public string no { get; set; }
        
        [Display(Name = "Plate")]
        public string Plate { get; set; }

        [Display(Name = "Cone")]
        public string Cone { get; set; }

        [Required]
        [Display(Name = "Visitor")]
        public string Visitor { get; set; }

        [Required]
        [Display(Name = "Idenfitication")]
        public string Idenfitication { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }
    }

    public class AccessCurrentViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Display(Name = "Plate")]
        public string Plate { get; set; }

        [Required]
        [Display(Name = "Cone")]
        public string Cone { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Street")]
        public string Street { get; set; }

        [Display(Name = "NumberHouse")]
        public string NumberHouse { get; set; }

        [Required]
        [Display(Name = "Photo")]
        public string Photo { get; set; }
        
    }
}