namespace Razor_Messenger.Hubs;

public interface IChatClient
{
    Task ReceiveMessage(string messageSender, string message, string messageTime);
    Task SendMessage(string message, string messageTime);
}