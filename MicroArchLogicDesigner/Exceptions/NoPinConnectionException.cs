namespace MicroArchLogicDesigner.Exceptions;

public class NoPinConnectionException : Exception
{
    public NoPinConnectionException(string? message) : base(message)
    {
    }
}
