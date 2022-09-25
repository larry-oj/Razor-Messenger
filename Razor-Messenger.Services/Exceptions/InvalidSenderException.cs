using System.Runtime.Serialization;

namespace Razor_Messenger.Services.Exceptions;

public class InvalidSenderException : Exception
{
    public InvalidSenderException()
        : base()
    {
    }
    
    public InvalidSenderException(string message) 
        : base(message)
    {
    }
    
    public InvalidSenderException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
    
    protected InvalidSenderException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    {
    }
}