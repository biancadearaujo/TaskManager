namespace TaskManager.Application.Exceptions;

public class ForbiddenException : ApplicationException 
{
    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException(string message, Exception inner) : base(message, inner)
    {
    }
}