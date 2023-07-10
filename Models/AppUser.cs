using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace book_store_server_side.Models
{ 
        public class AppUser : IdentityUser
        {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        
        public string Email { get; set; }

        }
    
}

