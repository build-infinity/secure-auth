using SecureAuth.Application.Common;
using SecureAuth.Application.Consts;
using SecureAuth.Application.Errors;
using SecureAuth.Domain.Models;

namespace SecureAuth.Application
{
    public static class OtpValidator
    {
        public static Error? Validate(EmailVerification? emailVerification)
        {
            if(emailVerification is null)
               return AuthErrors.InvalidVerificationId;
            if(emailVerification.IsUsed)
                return OtpErrors.Used;
            if(emailVerification.OtpExpiresOnUtc < DateTime.UtcNow)
                return OtpErrors.Expired;
            if(emailVerification.AttemptCount >= Otp.MaxAttempts)
                return OtpErrors.AttemptsExceeded;
                
            return null;
        }
    }
}