using System;
using System.ComponentModel.DataAnnotations;

namespace Suma.Authen.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public int UserId { get; set; }
        
        public DateTime? Revoked { get; set; }

        public DateTime Created { get; set; }
    }
}
