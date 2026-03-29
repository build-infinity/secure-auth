namespace SecureAuth.Application.Common
{
    public sealed record Error
    {
        public string Code { get; }
        public string Message { get; }
        public ErrorType Type {get; }
        public Error(string code, string messsage, ErrorType type)
        {
            Code = code;
            Message = messsage;
            Type = type;
        }
    }
}