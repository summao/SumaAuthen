using System;
using System.Threading.Tasks;
using Suma.Authen.Entities;
using Suma.Authen.Dtos;
using Suma.Authen.Dtos.Accounts;
using Suma.Authen.Repositories;
using BC = BCrypt.Net.BCrypt;
using Suma.Authen.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Suma.Authen.Services
{
    public interface IAccountService
    {
        Task<SignInResponse> SignInAsync(SignInRequest req);
        Task SignUpAsync(SignUpRequest reqModel, CancellationToken cancellationToken = default(CancellationToken));
        Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest reqModel);
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
                Role = Role.User,
                Created = DateTime.UtcNow,
            };

            await _accountRepository.InsertAsync(account, cancellationToken);
        }

        public async Task<SignInResponse> SignInAsync(SignInRequest reqModel)
        {
            var account = await _accountRepository.Collection.FirstOrDefaultAsync(x => x.MobileNumber == reqModel.MobileNumber
                && x.PasswordHash == BC.HashPassword(reqModel.Password)
            );
            if (account == null)
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

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest reqModel,  CancellationToken cancellationToken = default(CancellationToken))
        {
            var isRefreshTokenValid = await _refreshTokenService.IsRefreshTokenValid(reqModel.AccountId, reqModel.RefreshToken, cancellationToken);
            if (!isRefreshTokenValid)
            {
                return null;
            }

            var account = await _accountRepository.Collection.FirstOrDefaultAsync(a => a.Id == reqModel.AccountId, cancellationToken);
            return new RefreshTokenResponse
            {
                AccessToken = _jwtManager.GenerateAccessToken(account),
                RefreshToken = (await _refreshTokenService.Create(account.Id, cancellationToken)).Token,
            };
        }
    }
}