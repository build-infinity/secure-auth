namespace SecureAuth.Application.Consts
{
    public static class PasswordPolicy
    {
        public const int MinLength = 8;
        public const int MaxLength = 128;

        public const string SpecialChars = "!@#$%^&*()-_=+[]{};:'\",.<>/?\\|`~";
    }
}