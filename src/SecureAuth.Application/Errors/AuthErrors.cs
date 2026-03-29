using SecureAuth.Application.Common;

namespace SecureAuth.Application.Errors
{
    public static class AuthErrors
    {
        public static readonly Error InvalidCredentials = 
           new Error("AUTH_INVALID_CREDENTIALS", "Invalid email or password", ErrorType.Unauthorized);
        public static readonly Error InvalidVerificationId =
            new Error("AUTH_INVALID_VERIFICATION_ID", "VerificationId is invalid", ErrorType.Validation);
        public static readonly Error RefreshTokenInvalid = 
            new Error("AUTH_REFRESHTOKEN_INVALID","RefreshToken is invalid", ErrorType.Unauthorized);
        public static readonly Error RefreshTokenExpired = 
            new Error("AUTH_REFRESHTOKEN_EXPIRED","RefreshToken has expired", ErrorType.Unauthorized);
        public static readonly Error RefreshTokenAlreadyRevoked =
            new Error("AUTH_REFRESHTOKEN_ALREADY_REVOKED", "RefreshToken has already revoked", ErrorType.Unauthorized);
    }
}