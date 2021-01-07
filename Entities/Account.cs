using System;
using System.ComponentModel.DataAnnotations;

namespace SumaAuthen.Entities
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public Role Role { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}