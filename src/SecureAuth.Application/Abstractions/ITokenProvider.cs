using SecureAuth.Domain.Models;
using SecureAuth.Application.Common;

namespace SecureAuth.Application.Abstractions
{
    public interface ITokenProvider 
    {
       TokenResult GenerateAccessToken(User user);
       TokenResult GenerateRegistrationToken(string user);
       TokenResult GenerateRefreshToken();
    }
}