
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ScarlyCharter.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display (Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType (DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType (DataType.Password)]
        [Display (Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display (Name = "Payment Information")]
        public string PaymentInfo { get; set; }

        public string ReturnUrl { get; set; }
    }
}
