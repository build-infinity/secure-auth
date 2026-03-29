namespace SecureAuth.Application.Jobs
{
    public sealed record EmailJob
    {
        public string To { get; init;} = null!;
        public string Subject  {get; init;} = null!;
        public string Body { get; init; } = null!;
        public string? HtmlBody { get; init; } 
    }
}