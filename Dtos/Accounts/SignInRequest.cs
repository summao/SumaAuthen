using System.ComponentModel.DataAnnotations;

namespace Suma.Authen.Dtos.Accounts
{
    public class SignInRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}