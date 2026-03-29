using Microsoft.Extensions.Hosting;
using SecureAuth.Infrastructure.Channels;
using SecureAuth.Infrastructure.Email;

namespace SecureAuth.Infrastructure.BackgroundServices
{
    internal sealed class EmailBackgroundService : BackgroundService
    {
        private readonly EmailJobQueue _queue;
        private readonly IEmailSender _emailSender;

        public EmailBackgroundService(EmailJobQueue queue, IEmailSender emailSender)
        {
            _queue = queue;
            _emailSender = emailSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(await _queue.WaitForNextRead(stoppingToken))
            {
                try 
                {
                   var emailJob = await _queue.Dequeue(stoppingToken);
                   await _emailSender.SendAsync(emailJob.To, emailJob.Subject, emailJob.Body, emailJob.HtmlBody, stoppingToken);
                }
                catch(Exception ex) // custom exception needed
                {
                    Console.WriteLine("BackgroundService Exception : "+ ex.Message); // must be logged
                }
            }
        }
    }
}