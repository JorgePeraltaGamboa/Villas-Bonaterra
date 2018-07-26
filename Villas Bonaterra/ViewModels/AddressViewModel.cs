using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VillasBonaterra.Models;

namespace VillasBonaterra.ViewModels
{
    public class AddressUsersViewModel
    {
        [Required]
        [Display(Name = "Owner")]
        public string Owner { get; set; }
        public Guid Owner_Id { get; set; }
        public List<Usuario> Owners { get; set; }

        [Required]
        [Display(Name = "Streets")]
        public List<Calles> Streets { get; set; }

        [Required]
        [Display(Name = "Address")]
        public List<Domicilios> Address { get; set; }
        
    }
}