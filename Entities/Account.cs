using System;
using System.ComponentModel.DataAnnotations;

namespace Suma.Authen.Entities
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(200)]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProfileName { get; set; }

        [Required]
        [MaxLength(30)]
        public string Username { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        [Required]
        public Role Role { get; set; }

        public DateTime Created { get; set; }
        
        public DateTime? Updated { get; set; }
    }
}