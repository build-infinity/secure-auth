using SecureAuth.Application.Common;

namespace SecureAuth.Application.Errors
{
    public static class EmailErrors
    {
        public static readonly Error AlreadyVerified = 
            new Error("EMAIL_ALREADY_VERIFIED", "Email is already verified.", ErrorType.Conflict);
        public static readonly Error NotVerified = 
            new Error("EMAIL_NOT_VERIFIED", "Email is not verified.", ErrorType.Forbidden);
        public static readonly Error AlreadyExists = 
            new Error("EMAIL_ALREADY_EXISTS", "Email already exists.",ErrorType.Conflict);
    }
}