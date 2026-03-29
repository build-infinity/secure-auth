using SecureAuth.Application.Common;
using SecureAuth.Application.Errors;
using SecureAuth.Domain.Models;

namespace SecureAuth.Application.Validators
{
    public static class RefreshTokenValidator
    {
        public static Error? Validate(RefreshToken storedToken)
        {
            if(storedToken.IsRevoked)
               return AuthErrors.RefreshTokenAlreadyRevoked;
            if(storedToken.ExpiresOnUtc < DateTime.UtcNow)
                return AuthErrors.RefreshTokenExpired;
            return null; 
        }
    }
}