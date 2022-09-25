using System.Runtime.Serialization;

namespace Razor_Messenger.Services.Exceptions;

public class EmptyMessageException : Exception
{
    public EmptyMessageException()
        : base()
    {
    }
    
    public EmptyMessageException(string message) 
        : base(message)
    {
    }
    
    public EmptyMessageException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
    
    protected EmptyMessageException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    {
    }
}