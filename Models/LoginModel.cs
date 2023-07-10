using System;
using System.ComponentModel.DataAnnotations;

namespace book_store_server_side.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

