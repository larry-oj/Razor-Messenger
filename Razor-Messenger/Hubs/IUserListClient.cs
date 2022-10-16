namespace Razor_Messenger.Hubs;

public interface IUserListClient
{
    Task UpdateLastMessage(string username, string message, string messageTime);
    Task UpdateOnlineStatus(string username, bool isOnline);
    Task GetOnlineUsers(List<string> onlineUsers);
}