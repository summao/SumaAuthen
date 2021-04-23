using System.ComponentModel.DataAnnotations;

namespace Suma.Authen.Dtos
{
    public class SignUpRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProfileName { get; set; }

        [Required]
        [MaxLength(30)]
        [RegularExpression(@"^[a-z][a-z_\d]*[a-z1-9]$")]
        public string Username { get; set; }

        [Required]
        [Range(1,31)]
        public int Day { get; set; }

        [Required]
        [Range(1,12)]
        public int Month { get; set; }

        [Required]
        [Range(1990,2200)]
        public int Year { get; set; }
    }
}