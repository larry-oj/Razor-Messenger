namespace Razor_Messenger.Hubs;

public interface IUserListClient
{
    Task UpdateLastMessage(string username, string message, string messageTime);
}