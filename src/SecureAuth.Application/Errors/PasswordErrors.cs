using SecureAuth.Application.Common;
using SecureAuth.Application.Consts;

namespace SecureAuth.Application.Errors
{
    public static class PasswordErrors
    {
        public static Error MinLength(int minLength = PasswordPolicy.MinLength) =>
            new Error("PASSWORD_MIN_LENGTH", $"Password must be at least {minLength} characters", ErrorType.Validation);
        public static readonly Error RequireUppercase =
            new Error("PASSWORD_REQUIRE_UPPERCASE", "Password must contain at least one uppercase letter", ErrorType.Validation);
        public static readonly Error RequireLowercase =
            new Error("PASSWORD_REQUIRE_LOWERCASE", "Password must contain at least one lowercase letter", ErrorType.Validation);
        public static readonly Error RequireDigit =
            new Error("PASSWORD_REQUIRE_DIGIT", "Password must contain at least one digit", ErrorType.Validation);
        public static readonly Error RequireSpecialChar =
            new Error("PASSWORD_REQUIRE_SPECIAL_CHAR", "Password must contain at least one special character", ErrorType.Validation);
    }
}