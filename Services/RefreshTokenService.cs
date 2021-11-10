using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Suma.Authen.Entities;
using Suma.Authen.Helpers;
using Suma.Authen.Repositories;

namespace Suma.Authen.Services
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> Create(string accountId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> IsRefreshTokenValid(string accountId, string refreshTokenFromRequest, CancellationToken cancellationToken = default(CancellationToken));
    }

    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppSettings _appSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenService(
            IOptions<AppSettings> appSettings,
            IRefreshTokenRepository refreshTokenRepository
        )
        {
            _appSettings = appSettings.Value;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<RefreshToken> Create(string accountId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var refreshToken = new RefreshToken
            {
                Token = Utils.Random.RandomTokenString(),
                AccountId = accountId,
                Expired = System.DateTime.UtcNow.AddDays(_appSettings.RefreshTokenExpiredDays),
            };
            return await _refreshTokenRepository.InsertAsync(refreshToken, cancellationToken);
        }

        public async Task<bool> IsRefreshTokenValid(string accountId, string refreshTokenFromRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var refreshToken = await _refreshTokenRepository.GetOneAndDeleteAsync(a => a.AccountId == accountId && a.Token == refreshTokenFromRequest, cancellationToken);
            return refreshToken is not null;
        }
    }
}