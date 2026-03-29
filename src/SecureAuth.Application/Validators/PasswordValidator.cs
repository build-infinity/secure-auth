using SecureAuth.Application.Common;
using SecureAuth.Application.Consts;
using SecureAuth.Application.Errors;

namespace SecureAuth.Application.Validators
{
    public static class PasswordValidator
    {
        public static Error? Validate(string password)
        {
            
            if(password.Length < PasswordPolicy.MinLength)
                return PasswordErrors.MinLength();
            if(!password.Any(c => char.IsUpper(c)))
                return PasswordErrors.RequireUppercase;
            if(!password.Any(c => char.IsLower(c)))
                return PasswordErrors.RequireLowercase;
            if(!password.Any(c => char.IsDigit(c)))
                return PasswordErrors.RequireDigit;
            if(!password.Any(c => PasswordPolicy.SpecialChars.Contains(c))) 
                return PasswordErrors.RequireSpecialChar;
            
            return null;
        }
    }
}