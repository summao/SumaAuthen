using System.ComponentModel.DataAnnotations;

namespace Suma.Authen.Models
{
    public class SignUpRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(6)]
        public string Password { get; set; }

    }
}