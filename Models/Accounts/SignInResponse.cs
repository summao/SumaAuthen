using Suma.Authen.Entities;

namespace Suma.Authen.Models.Accounts
{
    public class SignInResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string JwtToken { get; set; }
    }
}