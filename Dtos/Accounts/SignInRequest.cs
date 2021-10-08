using System.ComponentModel.DataAnnotations;

namespace Suma.Authen.Dtos.Accounts
{
    public class SignInRequest
    {
        [Required]
        [MaxLength(20)]
        public string MobileNumber { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(6)]
        public string Password { get; set; }
    }
}