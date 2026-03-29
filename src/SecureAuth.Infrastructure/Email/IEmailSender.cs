namespace SecureAuth.Infrastructure.Email
{
    public interface IEmailSender 
    {
        Task SendAsync(string to, string subject, string body, string? htmlBody = null, CancellationToken ct = default);
    }
}