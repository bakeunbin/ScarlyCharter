﻿
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ScarlyCharter.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType (DataType.Password)]
        public string Password { get; set; }

        [Display (Name = "Remember me!")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
