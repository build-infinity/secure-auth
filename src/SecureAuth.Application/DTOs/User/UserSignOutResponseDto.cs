namespace SecureAuth.Application.DTOs.User
{
    public sealed record UserSignOutResponseDto
    {
        public string Message { get; init;} = null!;
    }
}