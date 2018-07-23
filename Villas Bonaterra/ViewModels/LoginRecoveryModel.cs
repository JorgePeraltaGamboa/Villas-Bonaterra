using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VillasBonaterra.ViewModels
{
    public class LoginRecoveryModel
    {
        [EmailAddress]
        [Required]
        public string email { get; set; }
    }
    public class RegisterUserModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required]
        public Guid Token { get; set; }

        [Required]
        public Guid Code { get; set; }
        
    }

    public class ChangePasswordConfirmModel
    {
        [Required]
        public Guid Token { get; set; }

        [Required]
        public Guid Code { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

    }

    public class ConfirmEmailModel {
        [Required]
        public string Token { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}