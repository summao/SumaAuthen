using System;
using System.Linq;
using System.Threading.Tasks;
using SumaAuthen.Entities;
using SumaAuthen.Models;
using SumaAuthen.Models.Accounts;
using SumaAuthen.Repositories;
using BC = BCrypt.Net.BCrypt;

namespace SumaAuthen.Services
{
    public interface IAccountService
    {
        Task<SignInResponse> SignInAsync(SignInRequest req);
        Task SignUpAsync(SignUpRequest reqModel);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepositories _accountRepositories;
        private readonly IJwtManager _jwtManager;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IAccountRepositories accountRepositories, IJwtManager jwtManager, IUnitOfWork unitOfWork)
        {
            _accountRepositories = accountRepositories;
            _jwtManager = jwtManager;
            _unitOfWork = unitOfWork;
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

        public async Task SignUpAsync(SignUpRequest reqModel)
        {
            var isDuplicated = _unitOfWork.Accounts.GetAsync(a => a.Email == reqModel.Email).Any();
            if (isDuplicated)
            {
                throw new ArgumentException("duplicated email");
            }

            var account = new Account
            {
                Email = reqModel.Email,
                PasswordHash = BC.HashPassword(reqModel.Password),
                Role = Role.User,
                Created = DateTime.UtcNow,
            };

            await _unitOfWork.Accounts.InsertAsync(account);
            await _unitOfWork.CommitAsync();
        }
    }
}