﻿using System.ComponentModel.DataAnnotations;

namespace shop
{
    public class UserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
        
        public string Role { get; set; } = "User";
    }
}
