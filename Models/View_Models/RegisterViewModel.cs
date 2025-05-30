﻿using System.ComponentModel.DataAnnotations;

namespace mvc_1.Models.View_Models
{
    public class RegisterViewModel
    {
            [Required]
            public string Username { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            [MinLength(6, ErrorMessage = "Password has to be atleast 6 characters")]
            public string Password { get; set; }
    }
}
