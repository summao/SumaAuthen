using System.ComponentModel.DataAnnotations;

namespace Suma.Authen.Dtos
{
    public class SignUpRequest
    {
        [Required]
        [MaxLength(20)]
        public string MobileNumber { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProfileName { get; set; }
    }
}