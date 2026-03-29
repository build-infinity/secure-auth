using SecureAuth.Application.Abstractions;
using SecureAuth.Application.Interfaces;
using SecureAuth.Application.Jobs;

namespace SecureAuth.Application.Services
{
    internal sealed class EmailService : IEmailService
    {
        private readonly IEmailJobQueue _emailJobQueue;

        public EmailService(IEmailJobQueue emailJobQueue)
        {
            _emailJobQueue = emailJobQueue;
        }

        public async Task SendOtp(string otp, string to)
        {
            EmailJob emailJob = new EmailJob()
            {
                To = to,
                Subject = "Email Conformation",
                Body = otp,
                HtmlBody = null
            };

            await _emailJobQueue.Enqueue(emailJob);
        }
    }
}