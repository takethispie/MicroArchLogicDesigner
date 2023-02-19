namespace MicroArchLogicDesigner.Exceptions;
public class DuplicatedInputsOnPinException : Exception
{
    public DuplicatedInputsOnPinException(string message) : base(message) { }
}
