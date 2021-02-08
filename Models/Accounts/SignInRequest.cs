using System.ComponentModel.DataAnnotations;

namespace Suma.Authen.Models.Accounts
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