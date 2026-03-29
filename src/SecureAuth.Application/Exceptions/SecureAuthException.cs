namespace SecureAuth.Application.Exceptions 
{
    public abstract class SecureAuthException : Exception
    {
        public string TechnicalMessage { get; }
        public string UserMessage { get; }
        public string ErrorCode { get; }

        public SecureAuthException(string technicalMessage, string userMessage, string errorCode) : base(technicalMessage)
        {
            TechnicalMessage = technicalMessage;
            UserMessage = userMessage;
            ErrorCode = errorCode;
        }
    }

}