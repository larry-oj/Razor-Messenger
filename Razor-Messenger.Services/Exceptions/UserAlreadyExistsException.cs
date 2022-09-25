using System.Runtime.Serialization;

namespace Razor_Messenger.Services.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException()
        : base()
    {
    }
    
    public UserAlreadyExistsException(string message) 
        : base(message)
    {
    }
    
    public UserAlreadyExistsException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
    
    protected UserAlreadyExistsException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    {
    }
}