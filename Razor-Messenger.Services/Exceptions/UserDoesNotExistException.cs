using System.Runtime.Serialization;

namespace Razor_Messenger.Services.Exceptions;

public class UserDoesNotExistException : Exception
{
    public UserDoesNotExistException()
        : base()
    {
    }
    
    public UserDoesNotExistException(string message) 
        : base(message)
    {
    }
    
    public UserDoesNotExistException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
    
    protected UserDoesNotExistException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    {
    }
}