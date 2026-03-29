using System.Threading.Channels;
using SecureAuth.Application.Abstractions;
using SecureAuth.Application.Jobs;

namespace SecureAuth.Infrastructure.Channels
{
    public class EmailJobQueue : IEmailJobQueue
    {
        private readonly Channel<EmailJob> _channel;

        public EmailJobQueue(Channel<EmailJob> channel)
        {
            _channel = channel;
        }
        public ValueTask Enqueue(EmailJob emailJob)
        {
            return _channel.Writer.WriteAsync(emailJob); 
        }
        public ValueTask<EmailJob> Dequeue(CancellationToken cancellationToken = default)
        {
           return _channel.Reader.ReadAsync(cancellationToken);
        }

        public ValueTask<bool> WaitForNextRead(CancellationToken cancellationToken = default)
        {
            return _channel.Reader.WaitToReadAsync(cancellationToken);
        }
    }
}