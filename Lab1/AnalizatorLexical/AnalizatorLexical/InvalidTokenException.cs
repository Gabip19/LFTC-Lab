namespace AnalizatorLexical;

public class InvalidTokenException : Exception
{
    public override string Message { get; }

    public InvalidTokenException()
    {
        Message = string.Empty;
    }

    public InvalidTokenException(string message)
        : base(message)
    {
        Message = message;
    }
    
    public InvalidTokenException(int lineNumber, string message)
    {
        Message = $"Exception on line {lineNumber}: {message}";
    }

    public InvalidTokenException(string message, Exception inner)
        : base(message, inner)
    {
        Message = message;
    }
}