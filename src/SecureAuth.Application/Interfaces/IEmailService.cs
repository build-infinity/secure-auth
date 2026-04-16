namespace SecureAuth.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendOtp(string otp, string to, CancellationToken cancellationToken);
    }
}