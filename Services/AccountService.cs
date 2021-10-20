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
            var account = await _accountRepository.Collection.FirstOrDefaultAsync(x =>
                x.MobileNumber == reqModel.MobileNumber
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

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest reqModel)
        {
            // var refreshToken = await _unitOfWork.RefreshTokens.GetOneAsync(a => a.Token == reqModel.RefreshToken && a.UserId == reqModel.UserId && a.Revoked == null);
            // if (refreshToken is null)
            // {
            //     return null;
            // }
            // refreshToken.Revoked = DateTime.UtcNow;

            // var account = await _unitOfWork.Accounts.GetOneAsync(a => a.Id == reqModel.UserId);
            // var accessToken = _jwtManager.GenerateJwtToken(account);
            // var newRefreshToken = new RefreshToken 
            // {
            //     Token = _jwtManager.RandomTokenString(),
            //     UserId = account.Id,
            // }; 

            // await _unitOfWork.RefreshTokens.InsertAsync(newRefreshToken);    
            // _unitOfWork.RefreshTokens.Update(refreshToken);
            // await _unitOfWork.CommitAsync();

            //  return new RefreshTokenResponse
            // {
            //     AccessToken = accessToken,
            //     RefreshToken = newRefreshToken.Token
            // };
            throw new NotImplementedException();
        }
    }
}