using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Suma.Authen.Entities;
using Suma.Authen.Helpers;

namespace Suma.Authen.Services
{
    public interface IJwtManager
    {
        string GenerateJwtToken(Account account);
        string RandomTokenString();
    }

    public class JwtManager : IJwtManager
    {
        private readonly AppSettings _appSettings;

        public JwtManager(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string GenerateJwtToken(Account account)
        {
            using var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(
                Convert.FromBase64String(_appSettings.RsaPrivateKey)
                , out _);

            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            DateTime jwtDate = DateTime.Now;
            var jwt = new JwtSecurityToken(
               audience: "jwt-test",
               issuer: "jwt-test",
               claims: new Claim[] { new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()) },
               notBefore: jwtDate,
               expires: jwtDate.AddSeconds(10),
               signingCredentials: signingCredentials
           );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}