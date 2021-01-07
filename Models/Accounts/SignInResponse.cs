using SumaAuthen.Entities;

namespace SumaAuthen.Models.Accounts
{
    public class SignInResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string JwtToken { get; set; }
    }
}