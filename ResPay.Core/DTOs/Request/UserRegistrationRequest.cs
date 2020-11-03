using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ResPay.Core.DTOs
{
    public class UserRegistrationRequest
    {
        [Required(ErrorMessage = "Please enter your firstname")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Please enter your lastname")]
        public string Lastname { get; set; }
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Please enter a valid phone number"), Required(ErrorMessage = "Please enter a valid phone number")]
        public string phoneNo { get; set; }
        [EmailAddress]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$")]
        [Required(ErrorMessage = "Please enter your email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("Password", ErrorMessage = "ConfirmPassword does not match Password")]
        public string confirmPassword { get; set; }
    }
}
