using System.Runtime.Serialization;

namespace Razor_Messenger.Services.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException()
        : base()
    {
    }
    
    public InvalidCredentialsException(string message) 
        : base(message)
    {
    }
    
    public InvalidCredentialsException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
    
    protected InvalidCredentialsException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    {
    }
}