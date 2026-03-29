using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SecureAuth.Application.Abstractions;

namespace SecureAuth.Infrastructure.Email 
{
    internal sealed class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpOptions _smtpOptions; 

        public SmtpEmailSender(IOptions<SmtpOptions> smtpOptions)
        {
            _smtpOptions = smtpOptions.Value;
        }

        public async Task SendAsync(string toEmail, string subject, string body, string? htmlBody = null, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(toEmail);
            ArgumentException.ThrowIfNullOrWhiteSpace(subject);
            ArgumentException.ThrowIfNullOrWhiteSpace(body);
            

            MimeMessage message = new MimeMessage();
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.From.Add(new MailboxAddress(_smtpOptions.FromName,_smtpOptions.FromEmail));
            message.Subject = subject;

            message.Body = htmlBody is null 
            ? new TextPart("plain")
            {
               Text = body
            }
            : new Multipart("alternative")
            {
               new TextPart("plain") { Text = body },
               new TextPart("html") { Text = htmlBody }
            };

            using SmtpClient smtpClient = new SmtpClient();
            
            await smtpClient.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, SecureSocketOptions.StartTls, ct);
            
            await smtpClient.AuthenticateAsync(_smtpOptions.User, _smtpOptions.Password, ct);

            await smtpClient.SendAsync(message, ct);

            await smtpClient.DisconnectAsync(true, ct);
        }
    }
}