using System;
using System.Threading.Tasks;
using Suma.Authen.Entities;
using Suma.Authen.Dtos;
using Suma.Authen.Dtos.Accounts;
using Suma.Authen.Repositories;
using BC = BCrypt.Net.BCrypt;
using Suma.Authen.Exceptions;
using System.Threading;

namespace Suma.Authen.Services
{
    public interface IAccountService
    {
        Task<SignInResponse> SignInAsync(SignInRequest req, CancellationToken cancellationToken = default(CancellationToken));
        Task SignUpAsync(SignUpRequest reqModel, CancellationToken cancellationToken = default(CancellationToken));
        Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest reqModel, CancellationToken cancellationToken = default(CancellationToken));
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IJwtManager _jwtManager;

        public AccountService(
            IAccountRepository accountRepository,
            IRefreshTokenService refreshTokenService,
            IJwtManager jwtManager)
        {
            _accountRepository = accountRepository;
            _refreshTokenService = refreshTokenService;
            _jwtManager = jwtManager;
        }

        public async Task SignUpAsync(SignUpRequest reqModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            var account = new Account
            {
                MobileNumber = reqModel.MobileNumber,
                PasswordHash = BC.HashPassword(reqModel.Password),
                ProfileName = reqModel.ProfileName,
                Username = reqModel.Username,
                Role = Role.User,
                Created = DateTime.UtcNow,
            };

            await _accountRepository.InsertAsync(account, cancellationToken);
        }

        public async Task<SignInResponse> SignInAsync(SignInRequest reqModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            var account = await _accountRepository.GetOneAsync(x => x.MobileNumber == reqModel.MobileNumber, cancellationToken);
            if (account is null || !BC.Verify(reqModel.Password, account.PasswordHash))
            {
                throw new SignInException("Email or password is incorrect");
            }

            return new SignInResponse
            {
                Id = account.Id,
                Role = account.Role,
                AccessToken = _jwtManager.GenerateAccessToken(account),
                RefreshToken = (await _refreshTokenService.Create(account.Id)).Token,
            };
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest reqModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            var isRefreshTokenValid = await _refreshTokenService.IsRefreshTokenValid(reqModel.AccountId, reqModel.RefreshToken, cancellationToken);
            if (!isRefreshTokenValid)
            {
                return null;
            }

            var account = await _accountRepository.GetOneAsync(a => a.Id == reqModel.AccountId, cancellationToken);
            return new RefreshTokenResponse
            {
                AccessToken = _jwtManager.GenerateAccessToken(account),
                RefreshToken = (await _refreshTokenService.Create(account.Id, cancellationToken)).Token,
            };
        }
    }
}