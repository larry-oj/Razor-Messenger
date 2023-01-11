namespace Razor_Messenger.Hubs;

public interface IChatClient
{
    Task ReceiveMessage(string messageSender, string message, string messageTime, string messageId);
    Task SendMessage(string message, string messageTime, string messageId);
    Task ReceiveEmotionAnalysis(string messageId, string emotion, string color);
    Task Blocked(string initiator, string target);
}