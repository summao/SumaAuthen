using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SumaAuthen.Helpers;
using SumaAuthen.Models.Accounts;
using SumaAuthen.Repositories;

namespace SumaAuthen.Services
{
    public interface IAccountService
    {
        Task<SignInResponse> SignInAsync(SignInRequest req);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepositories _accountRepositories;
        private readonly IJwtManager _jwtManager;

        public AccountService(IAccountRepositories accountRepositories, IJwtManager jwtManager)
        {
            _accountRepositories = accountRepositories;
            _jwtManager = jwtManager;
        }

        public async Task<SignInResponse> SignInAsync(SignInRequest req)
        {
            var account = await _accountRepositories.GetByEmailAsync(req.Email);
            if (account == null)
            {
                throw new Exception("Email or password is incorrect");
            }

            var jwtToken = _jwtManager.GenerateJwtToken(account);

            return new SignInResponse
            {
                Id = account.Id,
                Email = account.Email,
                Role = account.Role,
                JwtToken = jwtToken
            };
        }
    }
}