using SecureAuth.Application.Jobs;

namespace SecureAuth.Application.Abstractions
{
    public interface IEmailJobQueue
    {
        ValueTask Enqueue(EmailJob emailJob, CancellationToken cancellationToken = default);
    }
}