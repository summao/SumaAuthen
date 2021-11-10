namespace Suma.Authen.Dtos.Accounts
{
    public class RefreshTokenRequest
    {
        public string AccountId { get; set; }
        public string RefreshToken { get; set; }
    }
}