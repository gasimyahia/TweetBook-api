using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Domain;

namespace TweetBook4.Service
{
    public interface IIdentityService
    {
        Task<AuthenticatonResult> RegisterAsync(string email, string password);

        Task<AuthenticatonResult> LoginAsync(string email, string password);
        Task<AuthenticatonResult> RefreshTokenAsync(string token, string refreshToken);
    }
}
