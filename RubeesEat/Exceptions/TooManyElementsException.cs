namespace RubeesEat.Exceptions;

public class TooManyElementsException : Exception
{
    public TooManyElementsException() : base("Too many elements found.") {}
    public TooManyElementsException(string message) : base(message) {}
    public TooManyElementsException(string message, Exception innerException) 
        : base(message, innerException) {}
}
