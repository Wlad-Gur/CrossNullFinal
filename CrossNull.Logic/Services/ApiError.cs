namespace CrossNull.Logic.Services
{
    public enum ErrorTypes
    {
        Invalid,
        NotFound,
        InternalException,
    }

    public class ApiError
    {
        public ApiError(string message, ErrorTypes errorType)
        {
            Message = message;
            ErrorType = errorType;
        }
        public string Message { get; }

        public ErrorTypes ErrorType { get; }
    }
}