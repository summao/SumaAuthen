using System;
using System.Linq;
using System.Threading.Tasks;
using Suma.Authen.Entities;
using Suma.Authen.Dtos;
using Suma.Authen.Dtos.Accounts;
using Suma.Authen.Repositories;
using BC = BCrypt.Net.BCrypt;

namespace Suma.Authen.Services
{
    public interface IAccountService
    {
        Task<SignInResponse> SignInAsync(SignInRequest req);
        Task SignUpAsync(SignUpRequest reqModel);
        Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest reqModel);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtManager _jwtManager;

        public AccountService(IAccountRepository accountRepository, IJwtManager jwtManager)
        {
            _accountRepository = accountRepository;
            _jwtManager = jwtManager;
        }

        public async Task SignUpAsync(SignUpRequest reqModel)
        {
            var account = new Account
            {
                MobileNumber = reqModel.MobileNumber,
                PasswordHash = BC.HashPassword(reqModel.Password),
                ProfileName = reqModel.ProfileName,
                Role = Role.User,
                Created = DateTime.UtcNow,
            };

            await _accountRepository.InsertAsync(account);
        }

        public async Task<SignInResponse> SignInAsync(SignInRequest req)
        {
            // var account = await _accountRepositories.GetByEmailAsync(req.Email);
            // if (account == null)
            // {
            //     throw new Exception("Email or password is incorrect");
            // }

            // var jwtToken = _jwtManager.GenerateJwtToken(account);
            // var newRefreshToken = new RefreshToken 
            // {
            //     Token = _jwtManager.RandomTokenString(),
            //     UserId = account.Id,
            //     Created = DateTime.UtcNow,
            // }; 

            // await _unitOfWork.RefreshTokens.InsertAsync(newRefreshToken);
            // await _unitOfWork.CommitAsync();
            // return new SignInResponse
            // {
            //     Id = account.Id,
            //     Email = account.Email,
            //     Role = account.Role,
            //     AccessToken = jwtToken,
            //     RefreshToken = newRefreshToken.Token,
            // };
            throw new NotImplementedException();
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