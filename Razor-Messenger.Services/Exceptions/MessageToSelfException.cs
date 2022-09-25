using System.Runtime.Serialization;

namespace Razor_Messenger.Services.Exceptions;

public class MessageToSelfException : Exception
{
    public MessageToSelfException()
        : base()
    {
    }
    
    public MessageToSelfException(string message) 
        : base(message)
    {
    }
    
    public MessageToSelfException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
    
    protected MessageToSelfException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    {
    }
}