using System.Runtime.Serialization;

namespace Razor_Messenger.Services.Exceptions;

public class InvalidReceiverException : Exception
{
    public InvalidReceiverException()
        : base()
    {
    }
    
    public InvalidReceiverException(string message) 
        : base(message)
    {
    }
    
    public InvalidReceiverException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
    
    protected InvalidReceiverException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    {
    }
}