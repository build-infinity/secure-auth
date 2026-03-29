using SecureAuth.Application.Common;

namespace SecureAuth.Application.Errors
{
    public static class OtpErrors
    {
        public static readonly Error Invalid =
             new Error("OTP_INVALID", "OTP is invalid", ErrorType.Validation);
        public static readonly Error Expired =
             new Error("OTP_EXPIRED", "OTP has expired", ErrorType.Validation);
        public static readonly Error AttemptsExceeded = 
             new Error("OTP_ATTEMPTS_EXCEEDED", "Too many incorrect attempts. OTP is blocked", ErrorType.Validation);
        public static readonly Error Used = 
             new Error("OTP_USED", "OTP was already used", ErrorType.Validation);
        public static  Error AlreadySent(int remainingSeconds) => 
            new Error("OTP_ALREADY_SENT", $"OTP already sent, Try again in {remainingSeconds} seconds.", ErrorType.Conflict);

    }
}